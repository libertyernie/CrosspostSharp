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
using WeasylLib.Frontend;

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
					Image image = null;

					if (item is IRemotePhotoPost remote) {
						var req = WebRequestFactory.Create(remote.ThumbnailURL);
						using (var resp = await req.GetResponseAsync())
						using (var stream = resp.GetResponseStream())
						using (var ms = new MemoryStream()) {
							await stream.CopyToAsync(ms);
							ms.Position = 0;
							image = Image.FromStream(ms);
						}
					} else if (item is SavedPhotoPost saved) {
						using (var ms = new MemoryStream(saved.data, false)) {
							image = Image.FromStream(ms);
						}
					} else {
						image = picUserIcon.Image;
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
			picUserIcon.Image = null;
			lblUsername.Text = "";
			lblSiteName.Text = "";
			string avatarUrl = await _currentWrapper.GetUserIconAsync(picUserIcon.Width);

			if (avatarUrl != null) {
				var req = WebRequestFactory.Create(avatarUrl);
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					picUserIcon.Image = Image.FromStream(ms);
				}
			}

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

			var s = Settings.Load();
			if (s.DeviantArt.RefreshToken != null) {
				lblLoadStatus.Text = "Adding DeviantArt...";
				if (await UpdateDeviantArtTokens()) {
					var w = new DeviantArtSourceWrapper();
					var u = await w.GetUserAsync();

					add(w);
					add(new DeviantArtScrapsSourceWrapper(u.username));
					add(new DeviantArtStatusSourceWrapper());
					add(new OrderedAsyncSeqWrapper(new UnorderedStashSourceWrapper()));
				} else {
					MessageBox.Show(this, "DeviantArt refresh token is no longer valid", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					s.DeviantArt = new Settings.DeviantArtSettings {
						RefreshToken = null
					};
					s.Save();
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
				add(new InkbunnySourceWrapper(new InkbunnyLib.InkbunnyClient(i.sid, i.userId)));
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
			foreach (var p in s.Pixiv) {
				lblLoadStatus.Text = $"Adding Pixiv ({p.username})...";
				add(new PixivSourceWrapper(p.username, p.password));
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
			foreach (var w in s.Weasyl) {
				if (w.wzl == null) continue;

				lblLoadStatus.Text = $"Adding Weasyl ({w.username})...";

				var frontendClient = new WeasylFrontendClient() { WZL = w.wzl };
				var username = await frontendClient.GetUsernameAsync();
				add(new WeasylSourceWrapper(username, frontendClient));
				add(new WeasylCharacterSourceWrapper(username));
			}

			IEnumerable<IPostBase> imgur = Imgur.ImgurPostWrapper.AllPreviousUploads().ToList();
			if (imgur.Any()) {
				add(new AsyncSeqWrapperOfSeq("Previous Imgur uploads", imgur.Reverse()));
			}
			
			lblLoadStatus.Text = "Connecting to sites...";
			
			var tasks = list.Select(async c => {
				AsyncSeqWrapperPagedConsumer w = new AsyncSeqWrapperPagedConsumer(c, 4);
				try {
					var user = await c.GetUserAsync();
					return new WrapperMenuItem(w, string.IsNullOrEmpty(user.username)
						? c.Name
						: $"{user.username} - {c.Name}");
				} catch (Exception ex) {
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
			try {
				await ReloadWrapperList();
			} catch (Exception) {
				MessageBox.Show(this, "Could not load all source sites", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				lblLoadStatus.Visible = false;
			}
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
			Process.Start("https://github.com/libertyernie/CrosspostSharp/blob/v3.0/README.md");
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
	}
}
