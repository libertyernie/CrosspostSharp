using CrosspostSharp3.Imgur;
using CrosspostSharp3.Weasyl;
using DeviantArtControls;
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
		private SavedPhotoPost _downloaded;
		private IPostBase _origWrapper;

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
			_origWrapper = artwork;
			_downloaded = artwork as SavedPhotoPost;
			btnDelete.Enabled = _origWrapper is IDeletable;

			if (artwork is IRemotePhotoPost remote) {
				try {
					_downloaded = await PostConverter.DownloadAsync(remote);
				} catch (Exception ex) {
					Console.Error.WriteLine(ex);
					MessageBox.Show(this, $"Could not download remote photo", Text);
				}
			}

			btnView.Enabled = _origWrapper.ViewURL != null;
			saveAsToolStripMenuItem.Enabled = exportAsToolStripMenuItem.Enabled = (artwork is SavedPhotoPost x && x.url != null);

			if (_downloaded != null) {
				using (var ms = new MemoryStream(_downloaded.data, false)) {
					var image = Image.FromStream(ms);
					splitContainer1.Panel1.BackgroundImage = image;
					splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
				}
			}

			txtTitle.Text = artwork.Title;
			wbrDescription.Navigate("about:blank");
			wbrDescription.Document.Write($"<html><head></head><body>{artwork.HTMLDescription}</body></html>");
			wbrDescription.Document.Body.SetAttribute("contenteditable", "true");
			txtTags.Text = string.Join(" ", artwork.Tags);
			chkMature.Checked = artwork.Mature;
			chkAdult.Checked = artwork.Adult;

			BeginInvoke(new Action(ReloadOptions));
		}

		private void ReloadOptions() {
			Settings settings = Settings.Load();

			saveAsToolStripMenuItem.Enabled = false;
			exportAsToolStripMenuItem.Enabled = false;

			if (_downloaded != null) {
				var post = _downloaded;
				listBox1.Items.Add("--- Post as photo ---");

				saveAsToolStripMenuItem.Enabled = true;
				exportAsToolStripMenuItem.Enabled = true;

				if (settings.DeviantArt.RefreshToken != null) {
					listBox1.Items.Add(new DestinationOption("DeviantArt / Sta.sh", () => {
						using (var f = new Form()) {
							f.Width = 600;
							f.Height = 350;
							var d = new DeviantArtUploadControl {
								Dock = DockStyle.Fill
							};
							f.Controls.Add(d);
							d.Uploaded += url => f.Close();
							d.SetSubmission(
								post.data,
								txtTitle.Text,
								wbrDescription.Document.Body.InnerHtml,
								txtTags.Text.Split(' ').Where(s => s != ""),
								chkMature.Checked || chkAdult.Checked,
								post.url);
							f.ShowDialog(this);
						}
					}));
					listBox1.Items.Add(new DestinationOption("DeviantArt status update", () => {
						using (var f = new DeviantArtStatusUpdateForm(post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var fl in settings.Flickr) {
					listBox1.Items.Add(new DestinationOption($"Flickr ({fl.username})", () => {
						using (var f = new FlickrPostForm(fl, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var fn in settings.FurryNetwork) {
					listBox1.Items.Add(new DestinationOption($"Furry Network ({fn.characterName})", () => {
						using (var f = new FurryNetworkPostForm(fn, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var i in settings.Inkbunny) {
					listBox1.Items.Add(new DestinationOption($"Inkbunny ({i.username})", () => {
						using (var f = new InkbunnyPostForm(i, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var p in settings.Pillowfort) {
					listBox1.Items.Add(new DestinationOption($"Pillowfort ({p.username})", () => {
						using (var f = new PillowfortPostForm(p, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var t in settings.Twitter) {
					listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
						using (var f = new TwitterPostForm(t, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var t in settings.Tumblr) {
					listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
						using (var f = new TumblrPostForm(t, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				foreach (var w in settings.Weasyl) {
					if (w.wzl == null) continue;
					listBox1.Items.Add(new DestinationOption($"Weasyl ({w.username})", () => {
						using (var f = new WeasylPostForm(w, post)) {
							f.ShowDialog(this);
						}
					}));
				}
				if (File.Exists("efc.jar")) {
					listBox1.Items.Add(new DestinationOption($"FurAffinity / Weasyl", () => {
						LaunchEFC(post);
					}));
				}
				listBox1.Items.Add(new DestinationOption($"Imgur (anonymous upload)", async () => {
					if (MessageBox.Show(this, "Would you like to upload this image to Imgur?", Text, MessageBoxButtons.OKCancel) == DialogResult.OK) {
						try {
							var image = await ImgurAnonymousUpload.UploadAsync(post.data,
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

			listBox1.Items.Add("--- Post as text ---");

			if (settings.DeviantArt.RefreshToken != null) {
				listBox1.Items.Add(new DestinationOption("DeviantArt status update", () => {
					using (var f = new DeviantArtStatusUpdateForm(ExportAsText())) {
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
					using (var f = new TwitterNoPhotoPostForm(t, ExportAsText())) {
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

		private static void LaunchEFC(SavedPhotoPost artwork) {
			string jsonFile = Path.GetTempFileName();
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(new {
				artwork.adult,
				artwork.data,
				description = HtmlConversion.ConvertHtmlToText(artwork.description),
				artwork.mature,
				artwork.tags,
				artwork.title,
				artwork.url,
				contentType = PostConverter.GetContentType(artwork)
			}));

			Process process = Process.Start(new ProcessStartInfo("java", $"-jar efc.jar {jsonFile}") {
				RedirectStandardError = true,
				UseShellExecute = false,
				WorkingDirectory = Environment.CurrentDirectory,
				CreateNoWindow = true
			});
			process.EnableRaisingEvents = true;
			process.Exited += (o, a) => {
				if (process.ExitCode != 0) {
					string stderr = process.StandardError.ReadToEnd();
					MessageBox.Show(null, stderr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				if (jsonFile != null) File.Delete(jsonFile);
			};
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
			if (_downloaded == null) {
				MessageBox.Show(this, "This post does not have image data.", Text);
				return;
			}

			using (var saveFileDialog = new SaveFileDialog()) {
				saveFileDialog.Filter = "CrosspostSharp JSON|*.cps|All files|*.*";
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(_downloaded, Formatting.Indented));
				}
			}
		}

		private void exportAsToolStripMenuItem_Click(object sender, EventArgs e) {
			if (_downloaded == null) {
				MessageBox.Show(this, "This post does not have image data.", Text);
				return;
			}

			using (var saveFileDialog = new SaveFileDialog()) {
				using (var ms = new MemoryStream(_downloaded.data, false))
				using (var image = Image.FromStream(ms)) {
					saveFileDialog.Filter = image.RawFormat.Equals(ImageFormat.Png) ? "PNG images|*.png"
						: image.RawFormat.Equals(ImageFormat.Jpeg) ? "JPEG images|*.jpg;*.jpeg"
						: image.RawFormat.Equals(ImageFormat.Gif) ? "GIF images|*.gif"
						: "All files|*.*";
				}
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllBytes(saveFileDialog.FileName, _downloaded.data);
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

				if (d is IRemotePhotoPost p) {
					await ImgurAnonymousUpload.PromptForDeletionAsync(p.ImageURL);
				}
			}
		}

		private void btnView_Click(object sender, EventArgs e) {
			Process.Start(_origWrapper.ViewURL);
		}

		private void helpToolStripMenuItem1_Click(object sender, EventArgs e) {
			Process.Start("https://github.com/libertyernie/CrosspostSharp/blob/v3.0/README.md");
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
