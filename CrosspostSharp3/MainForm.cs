using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm : Form {
		private ISiteWrapper _currentWrapper;
		private int _currentPosition = 0;

		private async Task Populate() {
			if (_currentWrapper == null) return;

			tableLayoutPanel1.Controls.Clear();

			int i = _currentPosition;
			int stop = _currentPosition + (tableLayoutPanel1.RowCount * tableLayoutPanel1.ColumnCount);

			btnLoad.Enabled = false;
			btnPrevious.Enabled = false;
			btnNext.Enabled = false;

			try {
				while (true) {
					for (; i < stop && i < _currentWrapper.Cache.Count(); i++) {
						var item = _currentWrapper.Cache.Skip(i).First();

						Image image;
						var req = WebRequest.Create(item.ThumbnailURL);
						using (var resp = await req.GetResponseAsync())
						using (var stream = resp.GetResponseStream())
						using (var ms = new MemoryStream()) {
							await stream.CopyToAsync(ms);
							ms.Position = 0;
							image = Image.FromStream(ms);
						}

						tableLayoutPanel1.Controls.Add(new Panel {
							BackgroundImage = image,
							BackgroundImageLayout = ImageLayout.Zoom,
							Dock = DockStyle.Fill
						});
					}

					if (i == stop) break;

					await _currentWrapper.FetchAsync();
					if (_currentWrapper.IsEnded) break;
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			btnLoad.Enabled = true;
			btnPrevious.Enabled = _currentPosition > 0;
			btnNext.Enabled = _currentWrapper.Cache.Count() > stop || !_currentWrapper.IsEnded;
		}

		private async Task ReloadWrapperList() {
			ddlSource.Items.Clear();

			var s = Settings.Load();
			if (s.DeviantArt.RefreshToken != null) {
				ddlSource.Items.Add(new DeviantArtWrapper(new DeviantArtGalleryDeviationWrapper()));
				ddlSource.Items.Add(new StashWrapper());
			}
			if (s.FurAffinity.a != null && s.FurAffinity.b != null) {
				ddlSource.Items.Add(new FurAffinityWrapper(new FurAffinityIdWrapper(
					a: s.FurAffinity.a,
					b: s.FurAffinity.b)));
			}
		}

		public MainForm() {
			InitializeComponent();
		}

		private async void Form1_Shown(object sender, EventArgs e) {
			try {
				await ReloadWrapperList();
			} catch (Exception) {
				MessageBox.Show(this, "Could not load all source sites", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void btnLoad_Click(object sender, EventArgs e) {
			_currentWrapper = ddlSource.SelectedItem as ISiteWrapper;
			_currentPosition = 0;
			await Populate();
		}

		private async void btnPrevious_Click(object sender, EventArgs e) {
			_currentPosition = Math.Max(0, _currentPosition - 4);
			await Populate();
		}

		private async void btnNext_Click(object sender, EventArgs e) {
			_currentPosition += tableLayoutPanel1.RowCount + tableLayoutPanel1.ColumnCount;
			await Populate();
		}

		private async void furAffinityToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			try {
				Settings s = Settings.Load();
				if (s.FurAffinity.a != null && s.FurAffinity.b != null) {
					var w = new FurAffinityIdWrapper(a: s.FurAffinity.a, b: s.FurAffinity.b);
					try {
						var user = await w.WhoamiAsync();
						if (MessageBox.Show(this, $"You are currenty logged into FurAffinity as {user}. Would you like to log out?", Text, MessageBoxButtons.YesNo) == DialogResult.Yes) {
							s.FurAffinity.a = null;
							s.FurAffinity.b = null;
							s.Save();
							MessageBox.Show(this, "You have been logged out.", Text);
							await ReloadWrapperList();
						} else {
							return;
						}
					} catch (Exception) { }
				}
				using (var f = new FAWinFormsLogin.loginPages.LoginFormFA()) {
					f.Text = "Log In - FurAffinity";
					if (f.ShowDialog() == DialogResult.OK) {
						s.FurAffinity.a = f.ACookie;
						s.FurAffinity.b = f.BCookie;
						s.Save();
						await ReloadWrapperList();
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
