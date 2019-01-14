using DeviantArtFs;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using FurryNetworkLib;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm : Form {
		private IPagedWrapperConsumer _currentWrapper;
		private int _currentPosition = 0;

		private enum Direction { PREV, NEXT, FIRST };

		private async void Populate(Direction direction) {
			if (_currentWrapper == null) return;

			tableLayoutPanel1.Controls.Clear();

			btnLoad.Enabled = false;
			btnPrevious.Enabled = false;
			btnNext.Enabled = false;

			try {
				await UpdateAvatar();
			} catch (Exception ex) {
				Console.Error.WriteLine($"Could not load avatar: {ex.Message}");
			}

			bool more = true;

			try {
				var posts =
					direction == Direction.PREV ? await _currentWrapper.PrevAsync()
					: direction == Direction.NEXT ? await _currentWrapper.NextAsync()
					: direction == Direction.FIRST ? await _currentWrapper.FirstAsync()
					: throw new ArgumentException(nameof(direction));
				more = await _currentWrapper.HasMoreAsync();

				foreach (var item in posts) {
					var p = new PictureBox {
						SizeMode = PictureBoxSizeMode.Zoom,
						Cursor = Cursors.Hand,
						Dock = DockStyle.Fill
					};

					if (item is IRemotePhotoPost remote) {
						p.ImageLocation = remote.ThumbnailURL;
					} else if (item is SavedPhotoPost saved) {
						using (var ms = new MemoryStream(saved.data, false)) {
							p.Image = Image.FromStream(ms);
						}
					} else {
						p.ImageLocation = picUserIcon.ImageLocation;
					}

					p.Click += (o, e) => {
						using (var f = new ArtworkForm(item)) {
							f.ShowDialog(this);
						}
					};
					tableLayoutPanel1.Controls.Add(p);
				}
			} catch (Exception ex) {
				while (ex is AggregateException a && a.InnerExceptions.Count == 1) {
					ex = ex.InnerException;
				}
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			btnLoad.Enabled = true;
			btnPrevious.Enabled = _currentPosition > 0;
			btnNext.Enabled = more;
		}

		private async Task UpdateAvatar() {
			lblUsername.Text = "";
			lblSiteName.Text = "";
			picUserIcon.ImageLocation = await _currentWrapper.GetUserIconAsync(picUserIcon.Width);

			lblUsername.Text = await _currentWrapper.WhoamiAsync();
			lblSiteName.Text = _currentWrapper.Name;
		}

		private async Task ReloadWrapperList() {
			ddlSource.Items.Clear();

			var list = new List<AsyncSeqWrapper>();

			void add(AsyncSeqWrapper wrapper) {
				list.Add(wrapper);
			}

			lblLoadStatus.Visible = true;
			lblLoadStatus.Text = "Loading settings...";

			Settings s = Settings.Load();
			tableLayoutPanel1.Controls.Clear();
			tableLayoutPanel1.RowCount = s.MainForm?.rows ?? 2;
			tableLayoutPanel1.ColumnCount = s.MainForm?.columns ?? 2;
			tableLayoutPanel1.RowStyles.Clear();
			for (int i = 0; i < tableLayoutPanel1.RowCount; i++) {
				tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f / tableLayoutPanel1.RowCount));
			}
			tableLayoutPanel1.ColumnStyles.Clear();
			for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++) {
				tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f / tableLayoutPanel1.ColumnCount));
			}
			tsiPageSize4.Checked = tableLayoutPanel1.RowCount == 2 && tableLayoutPanel1.ColumnCount == 2;
			tsiPageSize9.Checked = tableLayoutPanel1.RowCount == 3 && tableLayoutPanel1.ColumnCount == 3;

			if (await s.UpdateTokensAsync()) {
				s.Save();
			}

			foreach (var da in s.DeviantArtAccounts) {
				lblLoadStatus.Text = $"Adding DeviantArt ({da.Username})...";
				var a = new DeviantArtAuth(OAuthConsumer.DeviantArt.CLIENT_ID, OAuthConsumer.DeviantArt.CLIENT_SECRET);
				try {
					var w = new DeviantArtSourceWrapper(da, includeLiterature: false);
					var u = await w.GetUserAsync();

					add(w);
					add(new DeviantArtScrapsLinkSourceWrapper(u.username, da));
					add(new DeviantArtStatusSourceWrapper(da));
					add(new OrderedAsyncSeqWrapper(new UnorderedStashSourceWrapper(da)));
				} catch (WebException ex) {
					throw new Exception("Could not load DeviantArt", ex);
				}
			}
			foreach (var fl in s.Flickr) {
				lblLoadStatus.Text = $"Adding Flickr {fl.username}...";
				add(new FlickrSourceWrapper(fl.CreateClient()));
			}
			foreach (var fa in s.FurAffinity) {
				lblLoadStatus.Text = $"Adding FurAffinity {fa.username}...";
				add(new FurAffinityMinimalSourceWrapper(
					a: fa.a,
					b: fa.b,
					scraps: false));
				add(new FurAffinityMinimalSourceWrapper(
					a: fa.a,
					b: fa.b,
					scraps: true));
			}
			foreach (var fn in s.FurryNetwork) {
				lblLoadStatus.Text = $"Adding Furry Network ({fn.characterName})...";
				var client = new FurryNetworkClient(fn.refreshToken);
				add(new FurryNetworkSourceWrapper(client, fn.characterName));
			}
			foreach (var i in s.Inkbunny) {
				lblLoadStatus.Text = $"Adding Inkbunny {i.username}...";
				add(new InkbunnySourceWrapper(new InkbunnyLib.InkbunnyClient(i.sid, i.userId), loadAll: false));
			}
			foreach (var p in s.Mastodon) {
				lblLoadStatus.Text = $"Adding Mastodon (@{p.username}@{p.appRegistration.Instance})...";
				add(new MastodonSourceWrapper(p.CreateClient(), photosOnly: true));
				add(new MastodonSourceWrapper(p.CreateClient(), photosOnly: false));
			}
			foreach (var t in s.Twitter) {
				lblLoadStatus.Text = $"Adding Twitter ({t.screenName})...";
				add(new TwitterSourceWrapper(t.GetCredentials(), photosOnly: true));
				add(new TwitterSourceWrapper(t.GetCredentials(), photosOnly: false));
			}
			foreach (var p in s.Pillowfort) {
				lblLoadStatus.Text = $"Adding Pillowfort ({p.username})...";
				add(new PillowfortSourceWrapper(new PillowfortFs.PillowfortClient { Cookie = p.cookie }));
			}
			TumblrClientFactory tcf = null;
			foreach (var t in s.Tumblr) {
				if (tcf == null) tcf = new TumblrClientFactory();
				lblLoadStatus.Text = $"Adding Tumblr ({t.blogName})...";
				var client = tcf.Create<TumblrClient>(
					OAuthConsumer.Tumblr.CONSUMER_KEY,
					OAuthConsumer.Tumblr.CONSUMER_SECRET,
					new DontPanic.TumblrSharp.OAuth.Token(t.tokenKey, t.tokenSecret));
				add(new TumblrSourceWrapper(client, t.blogName, photosOnly: true));
				add(new TumblrSourceWrapper(client, t.blogName, photosOnly: false));
			}
			foreach (var w in s.WeasylApi) {
				if (w.apiKey == null) continue;

				lblLoadStatus.Text = $"Adding Weasyl ({w.username})...";

				var wrapper = new WeasylSourceWrapper(w.apiKey, loadAll: false);
				var username = await wrapper.WhoamiAsync();
				add(wrapper);
				add(new WeasylCharacterSourceWrapper(username));
			}

			IEnumerable<IPostBase> imgur = Imgur.ImgurPostWrapper.AllPreviousUploads().ToList();
			if (imgur.Any()) {
				add(new AsyncSeqWrapperOfSeq("Previous Imgur uploads", imgur.Reverse()));
			}

			lblLoadStatus.Text = "Connecting to sites...";

			var tasks = list.Select(async c => {
				AsyncSeqWrapperPagedConsumer w = new AsyncSeqWrapperPagedConsumer(c, tableLayoutPanel1.RowCount * tableLayoutPanel1.ColumnCount);
				try {
					var user = await c.GetUserAsync();
					this.BeginInvoke(new Action(() => lblLoadStatus.Text += $" ({c.Name}: ok)"));
					return new WrapperMenuItem(w, string.IsNullOrEmpty(user.username)
						? c.Name
						: $"{user.username} - {c.Name}");
				} catch (Exception ex) {
					this.BeginInvoke(new Action(() => lblLoadStatus.Text += $" ({c.Name}: failed)"));
					var inner = ex;
					while (inner is AggregateException a) {
						inner = inner.InnerException;
					}
					if (inner is FurryNetworkClient.TokenException t) {
						return new WrapperMenuItem(w, $"{c.Name} (cannot connect: {t.Message})");
					} else {
						return new WrapperMenuItem(w, $"{c.Name} (cannot connect)");
					}
				}
			}).Where(item => item != null).ToArray();
			var wrappers = await Task.WhenAll(tasks);
			wrappers = wrappers
				.OrderBy(w => new string(w.DisplayName.Where(c => char.IsLetterOrDigit(c)).ToArray()))
				.ToArray();
			ddlSource.Items.AddRange(wrappers);

			lblLoadStatus.Visible = false;

			if (ddlSource.SelectedIndex < 0 && ddlSource.Items.Count > 0) {
				ddlSource.SelectedIndex = 0;
			}

			btnLoad.Enabled = ddlSource.Items.Count > 0;
		}

		public MainForm() {
			InitializeComponent();
		}

		private async void Form1_Shown(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;
			try {
				await ReloadWrapperList();
			} catch (Exception) {
				MessageBox.Show(this, "Could not load all source sites", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				lblLoadStatus.Visible = false;
			}
			toolsToolStripMenuItem.Enabled = true;
		}

		private void btnLoad_Click(object sender, EventArgs e) {
			_currentWrapper = (ddlSource.SelectedItem as WrapperMenuItem)?.BaseWrapper;
			_currentPosition = 0;
			Populate(Direction.FIRST);
		}

		private void btnPrevious_Click(object sender, EventArgs e) {
			_currentPosition = Math.Max(0, _currentPosition - 4);
			Populate(Direction.PREV);
		}

		private void btnNext_Click(object sender, EventArgs e) {
			_currentPosition += tableLayoutPanel1.RowCount + tableLayoutPanel1.ColumnCount;
			Populate(Direction.NEXT);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var openFileDialog = new OpenFileDialog()) {
				openFileDialog.Filter = ArtworkForm.OpenFilter;
				openFileDialog.Multiselect = false;
				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					using (var f = new ArtworkForm(openFileDialog.FileName)) {
						f.ShowDialog(this);
					}
				}
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void refreshAllToolStripMenuItem_Click(object sender, EventArgs e) {
			Populate(Direction.FIRST);
		}

		private void helpToolStripMenuItem1_Click(object sender, EventArgs e) {
			Process.Start("https://github.com/libertyernie/CrosspostSharp/blob/v3.5/README.md");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var f = new AboutForm()) {
				f.ShowDialog(this);
			}
		}

		private void exportToolStripMenuItem_Click_1(object sender, EventArgs e) {
			IEnumerable<IPagedWrapperConsumer> GetWrappers() {
				foreach (var o in ddlSource.Items) {
					if (o is WrapperMenuItem w) yield return w.BaseWrapper;
				}
			}

			using (var f = new BatchExportForm(GetWrappers())) {
				f.ShowDialog(this);
			}
		}

		private async void tsiPageSize4_Click(object sender, EventArgs e) {
			Settings s = Settings.Load();
			s.MainForm = new Settings.MainFormSettings {
				columns = 2,
				rows = 2,
			};
			s.Save();
			try {
				await ReloadWrapperList();
			} catch (Exception) {
				MessageBox.Show(this, "Could not load all source sites", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				lblLoadStatus.Visible = false;
			}
		}

		private async void tsiPageSize9_Click(object sender, EventArgs e) {
			Settings s = Settings.Load();
			s.MainForm = new Settings.MainFormSettings {
				columns = 3,
				rows = 3,
			};
			s.Save();
			try {
				await ReloadWrapperList();
			} catch (Exception) {
				MessageBox.Show(this, "Could not load all source sites", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				lblLoadStatus.Visible = false;
			}
		}
	}
}
