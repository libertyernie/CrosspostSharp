using ArtworkSourceSpecification;
using CrosspostSharp3.FurAffinity;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using CrosspostSharp3.FurryNetwork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrosspostSharp3.Weasyl;
using CrosspostSharp3.Twitter;
using CrosspostSharp3.Mastodon;
using CrosspostSharp3.Tumblr;
using CrosspostSharp3.DeviantArt;
using System.Threading;

namespace CrosspostSharp3 {
	public partial class MainForm : Form {
		private IArtworkSource _currentWrapper;
		private int _currentPosition = 0;

		private async void Populate() {
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
				var enumerable = _currentWrapper.GetPostsAsync();
				var posts = await enumerable.Skip(_currentPosition).Take(PageSize).ToListAsync();
				more = await enumerable.Skip(_currentPosition + PageSize).AnyAsync();

				foreach (var item in posts) {
					var p = new PictureBox {
						SizeMode = PictureBoxSizeMode.Zoom,
						Cursor = Cursors.Hand,
						Dock = DockStyle.Fill
					};

					if (item is IThumbnailPost t) {
						p.ImageLocation = t.ThumbnailURL;
					} else {
						p.ImageLocation = picUserIcon.ImageLocation;
					}

					p.Click += (o, e) => {
						using var f = new ArtworkForm(item);
						f.ShowDialog(this);
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
			var user = await _currentWrapper.GetUserAsync();

			lblUsername.Text = "";
			lblSiteName.Text = "";
			picUserIcon.ImageLocation = user.IconUrl;

			lblUsername.Text = user.Name;
			lblSiteName.Text = _currentWrapper.Name;
		}

		private readonly SemaphoreSlim _reloadSem = new(1, 1);

		private async Task ReloadWrapperList() {
			if (_reloadSem.CurrentCount == 0)
				return;

			await _reloadSem.WaitAsync();

			try {
				btnLoad.Enabled = false;

				ddlSource.Items.Clear();

				Settings s = Settings.Load();
				tableLayoutPanel1.Controls.Clear();
				tableLayoutPanel1.RowStyles.Clear();
				for (int i = 0; i < tableLayoutPanel1.RowCount; i++) {
					tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f / tableLayoutPanel1.RowCount));
				}
				tableLayoutPanel1.ColumnStyles.Clear();
				for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++) {
					tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f / tableLayoutPanel1.ColumnCount));
				}

				async IAsyncEnumerable<IArtworkSource> enumerateSources() {
					foreach (var da in s.DeviantArtTokens) {
						try { await DeviantArtFs.Api.Util.PlaceboAsync(da); } catch (Exception) { }
						yield return new DeviantArtGallerySource(da);
						yield return new PhotoPostFilterSource(new DeviantArtGallerySource(da));
						yield return new DeviantArtScrapsSource(da);
						yield return new DeviantArtStatusSource(da);
						yield return new StashSource(da);
					}
					foreach (var fa in s.FurAffinity) {
						yield return new FAExportArtworkSource(
							$"b={fa.b}; a={fa.a}",
							sfw: false,
							folder: "gallery");
						yield return new FAExportArtworkSource(
							$"b={fa.b}; a={fa.a}",
							sfw: false,
							folder: "scraps");
					}
					foreach (var fn in s.FurryNetwork) {
						var client = new FurryNetworkClient(fn.refreshToken);
						yield return new FurryNetworkArtworkSource(client, fn.characterName);
					}
					foreach (var i in s.Inkbunny) {
						yield return new Inkbunny.InkbunnyClient(i.sid, i.userId);
					}
					foreach (var p in s.Pixelfed) {
						var client = p.GetClient();
						yield return new MastodonSource(client);
						yield return new PhotoPostFilterSource(new MastodonSource(client));
					}
					foreach (var p in s.Pleronet) {
						var client = p.GetClient();
						yield return new MastodonSource(client);
						yield return new PhotoPostFilterSource(new MastodonSource(client));
					}
					foreach (var t in s.Twitter) {
						var source = new TwitterSource(t.GetCredentials());
						yield return source;
						yield return new PhotoPostFilterSource(source);
					}
					foreach (var t in s.Tumblr) {
						var client = new TumblrClientFactory().Create<TumblrClient>(
							OAuthConsumer.Tumblr.CONSUMER_KEY,
							OAuthConsumer.Tumblr.CONSUMER_SECRET,
							new DontPanic.TumblrSharp.OAuth.Token(t.tokenKey, t.tokenSecret));
						yield return new TumblrSource(client, t.blogName, PostType.Photo);
						yield return new TumblrSource(client, t.blogName, PostType.All);
					}
					foreach (var w in s.WeasylApi) {
						if (w.apiKey == null) continue;
						var client = new WeasylClient(w.apiKey);
						yield return new WeasylGallerySource(client);
						yield return new WeasylCharacterSource(client);
					}
				}

				await foreach (var c in enumerateSources()) {
					var w = new ArtworkCache(c);
					try {
						var user = await c.GetUserAsync();
						ddlSource.Items.Add(new WrapperMenuItem(w, string.IsNullOrEmpty(user.Name)
							? c.Name
							: $"{user.Name} - {c.Name}"));
					} catch (Exception ex) {
						Console.Error.WriteLine(ex);
					}
				}

				if (ddlSource.SelectedIndex < 0 && ddlSource.Items.Count > 0) {
					ddlSource.SelectedIndex = 0;
				}

				btnLoad.Enabled = ddlSource.Items.Count > 0;
			} finally {
				_reloadSem.Release();
			}
		}

		public MainForm() {
			InitializeComponent();
		}

		private void btnLoad_Click(object sender, EventArgs e) {
			_currentWrapper = (ddlSource.SelectedItem as WrapperMenuItem)?.BaseWrapper;
			_currentPosition = 0;
			Populate();
		}

		private int PageSize => tableLayoutPanel1.RowCount * tableLayoutPanel1.ColumnCount;

		private void btnPrevious_Click(object sender, EventArgs e) {
			_currentPosition = Math.Max(0, _currentPosition - PageSize);
			Populate();
		}

		private void btnNext_Click(object sender, EventArgs e) {
			_currentPosition += PageSize;
			Populate();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var openFileDialog = new OpenFileDialog()) {
				openFileDialog.Filter = ArtworkForm.OpenFilter;
				openFileDialog.Multiselect = false;
				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					using var f = new ArtworkForm(openFileDialog.FileName);
					f.ShowDialog(this);
				}
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void refreshAllToolStripMenuItem_Click(object sender, EventArgs e) {
			Populate();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using var f = new AboutForm();
			f.ShowDialog(this);
		}

		private void exportToolStripMenuItem_Click_1(object sender, EventArgs e) {
			IEnumerable<IArtworkSource> GetWrappers() {
				foreach (var o in ddlSource.Items) {
					if (o is WrapperMenuItem w) yield return w.BaseWrapper;
				}
			}

			using var f = new BatchExportForm(GetWrappers());
			f.ShowDialog(this);
		}

		private void postToolStripMenuItem_Click(object sender, EventArgs e) {
			using var f = new StatusPostForm();
			f.ShowDialog(this);
		}

		private async void ddlSource_DropDown(object sender, EventArgs e) {
			if (ddlSource.Items.Count == 0) {
				try {
					await ReloadWrapperList();
				} catch (Exception ex) {
					Console.Error.WriteLine(ex);
					MessageBox.Show(this, "Could not load all source sites", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
