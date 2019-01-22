using CrosspostSharp3.Imgur;
using CrosspostSharp3.Weasyl;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Newtonsoft.Json;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class ArtworkForm : Form {
		private IPostBase _origWrapper;
		private SavedPhotoPost _savedPhotoPost;

		private class DestinationOption {
			public readonly string Name;
			public readonly Action Click;

			public DestinationOption(string name, Action click) {
				Name = name;
				Click = click;
			}

			public override string ToString() {
				return Name;
			}
		}

		private class TextPost : IPostBase {
			public string Title { get; set; }
			public string HTMLDescription { get; set; }
			public bool Mature { get; set; }
			public bool Adult { get; set; }
			public IEnumerable<string> Tags { get; set; }
			public string ViewURL { get; set; }

			DateTime IPostBase.Timestamp => DateTime.UtcNow;

			public TextPost() { }
		}

		public IPostBase ExportAsText() {
			return new TextPost {
				Title = txtTitle.Text,
				HTMLDescription = wbrDescription.Document.Body.InnerHtml,
				Tags = txtTags.Text.Split(' ').Where(s => s != ""),
				Mature = chkMature.Checked,
				Adult = chkAdult.Checked,
				ViewURL = _origWrapper.ViewURL
			};
		}

		public SavedPhotoPost ExportAsPhoto() {
			if (_savedPhotoPost == null) throw new Exception("Not a saved photo post");

			return new SavedPhotoPost(
				_savedPhotoPost.data,
				txtTitle.Text,
				wbrDescription.Document.Body.InnerHtml,
				_origWrapper.ViewURL,
				txtTags.Text.Split(' ').Where(s => s != ""),
				chkMature.Checked,
				chkAdult.Checked);
		}

		public ArtworkForm() {
			InitializeComponent();

			Shown += (o, e) => {
				if (Owner is MainForm) {
					mainWindowAccountSetupToolStripMenuItem.Enabled = false;
				}
			};
		}

		public ArtworkForm(string filename) : this() {
			this.Shown += (o, e) => LoadImage(filename);
		}

		public ArtworkForm(IPostBase post) : this() {
			this.Shown += (o, e) => LoadImage(post);
		}

		public void LoadImage(string filename) {
			LoadImage(PostConverter.FromFile(filename));
		}

		public async void LoadImage(IPostBase artwork) {
			// Store original post object (used for View and Delete actions)
			_origWrapper = artwork;
			btnView.Enabled = _origWrapper.ViewURL != null;
			btnDelete.Enabled = _origWrapper is IDeletable;

			// Convert DeferredPhotoPost to IRemotePhotoPost
			if (_origWrapper is DeferredPhotoPost deferred) {
				for (int i = 0; i < Controls.Count; i++) {
					Controls[i].Enabled = false;
				}
				string origText = this.Text;
				this.Text = "Loading...";

				try {
					_origWrapper = await deferred.GetActualAsync();
				} catch (Exception) { }

				if (this.IsDisposed) return;

				this.Text = origText;
				for (int i = 0; i < Controls.Count; i++) {
					Controls[i].Enabled = true;
				}
			}

			// Convert IRemotePhotoPost to SavedPhotoPost
			// This does not maintain the view and delete functions, so we'll use a separate variable
			_savedPhotoPost = _origWrapper is SavedPhotoPost p1 ? p1
				: _origWrapper is IRemotePhotoPost p2 ? await PostConverter.DownloadAsync(p2)
				: null;

			// Get photo (or thumbnail of video)
			if (_savedPhotoPost != null) {
				using (var ms = new MemoryStream(_savedPhotoPost.data, false)) {
					var image = Image.FromStream(ms);
					pictureBox1.Image = image;
				}
			} else if (_origWrapper is IThumbnailPost thumb) {
				pictureBox1.ImageLocation = thumb.ThumbnailURL;
			}

			btnView.Enabled = _origWrapper.ViewURL != null;
			saveAsToolStripMenuItem.Enabled = exportAsToolStripMenuItem.Enabled = (_origWrapper is SavedPhotoPost x && x.url != null);
			txtTitle.Text = _origWrapper.Title;
			wbrDescription.Navigate("about:blank");
			wbrDescription.Document.Write($"<html><head></head><body>{_origWrapper.HTMLDescription}</body></html>");
			wbrDescription.Document.Body.SetAttribute("contenteditable", "true");
			txtTags.Text = string.Join(" ", _origWrapper.Tags);
			chkMature.Checked = _origWrapper.Mature;
			chkAdult.Checked = _origWrapper.Adult;

			Settings settings = Settings.Load();

			if (await settings.UpdateTokensAsync()) {
				settings.Save();
			}

			saveAsToolStripMenuItem.Enabled = false;
			exportAsToolStripMenuItem.Enabled = false;

			if (_savedPhotoPost?.data != null) {
				listBox1.Items.Add("--- Post as photo ---");

				saveAsToolStripMenuItem.Enabled = true;
				exportAsToolStripMenuItem.Enabled = true;

				foreach (var da in settings.DeviantArtAccounts) {
					listBox1.Items.Add(new DestinationOption($"DeviantArt / Sta.sh {da.Username}", () => {
						var photo = ExportAsPhoto();
						long? itemId = (_origWrapper as StashPostWrapper)?.ItemId;
						if (PostConverter.GetContentType(photo) == "image/gif") {
							switch (MessageBox.Show(this, "GIF images on DeviantArt require a separate preview image, which isn't possible via the API. Would you like to upload this image in PNG format instead?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
								case DialogResult.Cancel:
									return;
								case DialogResult.Yes:
									using (var ms = new MemoryStream(photo.data))
									using (var image = Image.FromStream(ms))
									using (var ms2 = new MemoryStream()) {
										image.Save(ms2, ImageFormat.Png);
										photo = PostConverter.ReplaceData(ms2.ToArray(), photo);
										itemId = null;
									}
									break;
							}
						}
						using (var f = new Form()) {
							f.Width = 600;
							f.Height = 350;
							var d = new DeviantArtUploadControl(da) {
								Dock = DockStyle.Fill
							};
							f.Controls.Add(d);
							d.Uploaded += url => f.Close();
							d.SetSubmission(photo, itemId);
							f.ShowDialog(this);
						}
					}));
					listBox1.Items.Add(new DestinationOption($"DeviantArt status update ({da.Username})", () => {
						using (var f = new DeviantArtStatusUpdateForm(da, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var fl in settings.Flickr) {
					listBox1.Items.Add(new DestinationOption($"Flickr ({fl.username})", () => {
						using (var f = new FlickrPostForm(fl, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var fa in settings.FurAffinity) {
					listBox1.Items.Add(new DestinationOption($"Fur Affinity ({fa.username})", () => {
						using (var f = new FurAffinityPostForm(fa, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var fn in settings.FurryNetwork) {
					listBox1.Items.Add(new DestinationOption($"Furry Network ({fn.characterName})", () => {
						using (var f = new FurryNetworkPostForm(fn, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var i in settings.Inkbunny) {
					listBox1.Items.Add(new DestinationOption($"Inkbunny ({i.username})", () => {
						using (var f = new InkbunnyPostForm(i, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var m in settings.Mastodon) {
					listBox1.Items.Add(new DestinationOption($"{m.Instance} ({m.username})", () => {
						using (var f = new MastodonCwPostForm(m, ExportAsPhoto(), _origWrapper)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var p in settings.Pillowfort) {
					listBox1.Items.Add(new DestinationOption($"Pillowfort ({p.username})", () => {
						using (var f = new PillowfortPostForm(p, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var t in settings.Twitter) {
					listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
						using (var f = new TwitterPostForm(t, ExportAsPhoto(), _origWrapper)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var t in settings.Tumblr) {
					listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
						using (var f = new TumblrPostForm(t, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var w in settings.WeasylApi) {
					if (w.apiKey == null) continue;
					listBox1.Items.Add(new DestinationOption($"Weasyl ({w.username})", () => {
						using (var f = new WeasylPostForm(w, ExportAsPhoto())) {
							f.ShowDialog(this);
						}
					}));
				}
				listBox1.Items.Add(new DestinationOption($"Imgur (anonymous upload)", async () => {
					if (MessageBox.Show(this, "Would you like to upload this image to Imgur?", Text, MessageBoxButtons.OKCancel) == DialogResult.OK) {
						try {
							var image = await ImgurAnonymousUpload.UploadAsync(_savedPhotoPost.data,
								title: txtTitle.Text,
								description: wbrDescription.Document.Body.InnerHtml);
							Process.Start(image);
						} catch (Exception ex) {
							Console.Error.WriteLine(ex);
							MessageBox.Show(this, "Could not upload to Imgur (an unknown error occured.)");
						}
					}
				}));
				listBox1.Items.Add("");
			}

			if (_origWrapper is IRemoteVideoPost video) {
				listBox1.Items.Add("--- Post as video ---");
				foreach (var m in settings.Mastodon) {
					listBox1.Items.Add(new DestinationOption($"{m.Instance} ({m.username})", () => {
						using (var f = new MastodonCwPostForm(m, ExportAsText(), _origWrapper)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var t in settings.Twitter) {
					listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
						using (var f = new TwitterPostForm(t, ExportAsText(), _origWrapper)) {
							f.ShowDialog(this);
						}
					}));
				}
				listBox1.Items.Add("");
			}

			listBox1.Items.Add("--- Post as text ---");

			foreach (var da in settings.DeviantArtAccounts) {
				listBox1.Items.Add(new DestinationOption($"DeviantArt status update ({da.Username})", () => {
					using (var f = new DeviantArtStatusUpdateForm(da, ExportAsText())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var m in settings.Mastodon) {
				listBox1.Items.Add(new DestinationOption($"{m.Instance} ({m.username})", () => {
					using (var f = new MastodonCwPostForm(m, ExportAsText())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var p in settings.Pillowfort) {
				listBox1.Items.Add(new DestinationOption($"Pillowfort ({p.username})", () => {
					using (var f = new PillowfortPostForm(p, ExportAsText())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Twitter) {
				listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
					using (var f = new TwitterPostForm(t, ExportAsText())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Tumblr) {
				listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
					using (var f = new TumblrNoPhotoPostForm(t, ExportAsText())) {
						f.ShowDialog(this);
					}
				}));
			}
		}

		private void btnPost_Click(object sender, EventArgs ea) {
			var o = listBox1.SelectedItem as DestinationOption;
			o?.Click?.Invoke();
		}

		public const string OpenFilter = "All supported formats|*.png;*.jpg;*.jpeg;*.gif;*.cps|Image files|*.png;*.jpg;*.jpeg;*.gif|CrosspostSharp JSON|*.cps|All files|*.*";

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var openFileDialog = new OpenFileDialog()) {
				openFileDialog.Filter = OpenFilter;
				openFileDialog.Multiselect = false;
				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					LoadImage(openFileDialog.FileName);
				}
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			if (_savedPhotoPost == null) {
				MessageBox.Show(this, "This post does not have image data.", Text);
				return;
			}

			using (var saveFileDialog = new SaveFileDialog()) {
				saveFileDialog.Filter = "CrosspostSharp JSON|*.cps|All files|*.*";
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(ExportAsPhoto(), Formatting.Indented));
				}
			}
		}

		private void exportAsToolStripMenuItem_Click(object sender, EventArgs e) {
			if (_savedPhotoPost == null) {
				MessageBox.Show(this, "This post does not have image data.", Text);
				return;
			}

			using (var saveFileDialog = new SaveFileDialog()) {
				using (var ms = new MemoryStream(_savedPhotoPost.data, false))
				using (var image = Image.FromStream(ms)) {
					saveFileDialog.Filter = image.RawFormat.Equals(ImageFormat.Png) ? "PNG images|*.png"
						: image.RawFormat.Equals(ImageFormat.Jpeg) ? "JPEG images|*.jpg;*.jpeg"
						: image.RawFormat.Equals(ImageFormat.Gif) ? "GIF images|*.gif"
						: "All files|*.*";
				}
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllBytes(saveFileDialog.FileName, _savedPhotoPost.data);
				}
			}
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void listBox1_DoubleClick(object sender, EventArgs e) {
			btnPost.PerformClick();
		}

		private async void btnDelete_Click(object sender, EventArgs e) {
			if (_origWrapper is IDeletable d) {
				if (MessageBox.Show(this, $"Are you sure you want to permanently delete this submission from {d.SiteName}?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
					try {
						await d.DeleteAsync();
						Close();
					} catch (Exception ex) {
						MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void btnView_Click(object sender, EventArgs e) {
			Process.Start(_origWrapper.ViewURL);
		}

		private void helpToolStripMenuItem1_Click(object sender, EventArgs e) {
			Process.Start("https://github.com/libertyernie/CrosspostSharp/blob/v3.5/README.md");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var f = new AboutForm()) {
				f.ShowDialog(this);
			}
		}

		private void mainWindowAccountSetupToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var f = new MainForm()) {
				f.ShowDialog(this);
			}
		}
	}
}
