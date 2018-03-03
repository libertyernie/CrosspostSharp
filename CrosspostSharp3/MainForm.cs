﻿using ArtSourceWrapper;
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
				await UpdateAvatar();
			} catch (Exception ex) {
				Console.Error.WriteLine($"Could not load avatar: {ex.Message}");
			}

			try {
				while (true) {
					for (; i < stop && i < _currentWrapper.Cache.Count(); i++) {
						var item = _currentWrapper.Cache.Skip(i).First();

						Image image;
						var req = WebRequestFactory.Create(item.ThumbnailURL);
						using (var resp = await req.GetResponseAsync())
						using (var stream = resp.GetResponseStream())
						using (var ms = new MemoryStream()) {
							await stream.CopyToAsync(ms);
							ms.Position = 0;
							image = Image.FromStream(ms);
						}

						var p = new Panel {
							BackgroundImage = image,
							BackgroundImageLayout = ImageLayout.Zoom,
							Cursor = Cursors.Hand,
							Dock = DockStyle.Fill
						};
						p.Click += (o, e) => {
							using (var f = new ArtworkForm(item)) {
								f.ShowDialog(this);
							}
						};
						tableLayoutPanel1.Controls.Add(p);
					}

					if (i == stop) break;

					if (_currentWrapper.IsEnded) break;
					await _currentWrapper.FetchAsync();
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			btnLoad.Enabled = true;
			btnPrevious.Enabled = _currentPosition > 0;
			btnNext.Enabled = _currentWrapper.Cache.Count() > stop || !_currentWrapper.IsEnded;
		}

		private async Task UpdateAvatar() {
			picUserIcon.Image = null;
			lblUsername.Text = "";
			lblSiteName.Text = "";
			string avatarUrl = await _currentWrapper.GetUserIconAsync(picUserIcon.Width);
			if (avatarUrl == null) return;

			var req = WebRequestFactory.Create(avatarUrl);
			using (var resp = await req.GetResponseAsync())
			using (var stream = resp.GetResponseStream())
			using (var ms = new MemoryStream()) {
				await stream.CopyToAsync(ms);
				ms.Position = 0;
				picUserIcon.Image = Image.FromStream(ms);
			}

			lblUsername.Text = await _currentWrapper.WhoamiAsync();
			lblSiteName.Text = _currentWrapper.SiteName;
		}

		private async Task ReloadWrapperList() {
			ddlSource.Items.Clear();

			var list = new List<ISiteWrapper>();

			var s = Settings.Load();
			if (s.DeviantArt.RefreshToken != null) {
				if (await UpdateDeviantArtTokens()) {
					list.Add(new DeviantArtWrapper(new DeviantArtGalleryDeviationWrapper()));
					list.Add(new StashWrapper());
				}
			}
			if (s.FurAffinity.a != null && s.FurAffinity.b != null) {
				list.Add(new FurAffinityWrapper(new FurAffinityIdWrapper(
					a: s.FurAffinity.a,
					b: s.FurAffinity.b)));
			}

			var tasks = list.Select(async w => new WrapperMenuItem(w, $"{await w.WhoamiAsync()} - {w.WrapperName}")).ToArray();
			var wrappers = await Task.WhenAll(tasks);
			wrappers = wrappers.OrderBy(w => w.DisplayName).ToArray();
			ddlSource.Items.AddRange(wrappers);

			if (ddlSource.SelectedIndex < 0) {
				ddlSource.SelectedIndex = 0;
			}

			btnLoad.Enabled = ddlSource.Items.Count > 0;
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
			_currentWrapper = (ddlSource.SelectedItem as WrapperMenuItem)?.BaseWrapper;
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

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var openFileDialog = new OpenFileDialog()) {
				openFileDialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.gif";
				openFileDialog.Multiselect = false;
				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					using (var f = new ArtworkForm(File.ReadAllBytes(openFileDialog.FileName))) {
						f.ShowDialog(this);
					}
				}
			}
		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e) {
			throw new NotImplementedException();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}
	}
}
