using ArtworkSourceSpecification;
using CrosspostSharp3.DeviantArt;
using CrosspostSharp3.FurAffinity;
using CrosspostSharp3.FurryNetwork;
using CrosspostSharp3.Inkbunny;
using CrosspostSharp3.Mastodon;
using CrosspostSharp3.Tumblr;
using CrosspostSharp3.Twitter;
using CrosspostSharp3.Weasyl;
using DeviantArtFs.Api;
using Microsoft.FSharp.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class ArtworkForm : Form {
		private IDownloadedData _exportable;

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

		public TextPost ExportAsText() {
			return new TextPost {
				Title = txtTitle.Text,
				HTMLDescription = wbrDescription.Document.Body.InnerHtml,
				Tags = ListModule.OfSeq(txtTags.Text.Split(' ').Where(s => s != "")),
				Mature = chkMature.Checked,
				Adult = chkAdult.Checked
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

		public record LocalFile(string Filename) : IDownloadedData {
			string Stash.IFormFile.ContentType {
				get {
					using var image = Image.FromFile(Filename);
					return image.RawFormat.Guid == ImageFormat.Png.Guid ? "image/png"
						: image.RawFormat.Guid == ImageFormat.Jpeg.Guid ? "image/jpeg"
						: image.RawFormat.Guid == ImageFormat.Gif.Guid ? "image/gif"
						: "application/octet-stream";
				}
			}

			byte[] Stash.IFormFile.Data => File.ReadAllBytes(Filename);
		}

		public void LoadImage(string filename) => LoadImage(new LocalFile(filename));

		public async void LoadImage(IPostBase artwork) {
			IDownloadedData downloadedData;
			try {
				downloadedData = await Downloader.DownloadAsync(artwork);
			} catch (WebException ex) {
				MessageBox.Show(this, ex.Message);
				downloadedData = null;
			}

			LoadImage(downloadedData,
				title: artwork.Title,
				htmlDescription: artwork.HTMLDescription,
				tags: artwork.Tags,
				mature: artwork.Mature,
				adult: artwork.Adult,
				relativeTo: Uri.TryCreate(artwork.ViewURL, UriKind.Absolute, out Uri u) ? u : null);
		}

		public void LoadImage(IDownloadedData downloadedData, string title = "", string htmlDescription = "", IEnumerable<string> tags = null, bool mature = false, bool adult = false, Uri relativeTo = null) {
			_exportable = downloadedData;

			using var ms = new MemoryStream(downloadedData.Data, false);
			pictureBox1.Image = Image.FromStream(ms);

			txtTitle.Text = title;
			wbrDescription.Navigate("about:blank");
			wbrDescription.Document.Write($"<html><head></head><body>{htmlDescription}</body></html>");
			wbrDescription.Document.Body.SetAttribute("contenteditable", "true");
			txtTags.Text = string.Join(" ", tags ?? Enumerable.Empty<string>());
			chkMature.Checked = mature;
			chkAdult.Checked = adult;

			try {
				if (relativeTo != null) {
					var links = wbrDescription.Document.GetElementsByTagName("a");
					for (int i = 0; i < links.Count; i++) {
						var a = links[i];
						string href = a.GetAttribute("href");
						if (href.StartsWith("about:"))
							href = href[6..];
						if (Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out Uri u2)) {
							a.SetAttribute("href", new Uri(relativeTo, u2).AbsoluteUri);
						}
					}

					var images = wbrDescription.Document.GetElementsByTagName("img");
					for (int i = 0; i < images.Count; i++) {
						var img = images[i];
						string src = img.GetAttribute("src");
						if (src.StartsWith("about:"))
							src = src[6..];
						if (Uri.TryCreate(src, UriKind.RelativeOrAbsolute, out Uri u2)) {
							img.SetAttribute("src", new Uri(relativeTo, u2).AbsoluteUri);
						}
					}
				}
			} catch (Exception ex) {
				Console.Error.WriteLine(ex);
			}

			Settings settings = Settings.Load();

			exportAsToolStripMenuItem.Enabled = false;

			if (downloadedData != null) {
				listBox1.Items.Add("--- Post as photo ---");

				exportAsToolStripMenuItem.Enabled = true;

				foreach (var da in settings.DeviantArtTokens) {
					listBox1.Items.Add(new DestinationOption($"DeviantArt / Sta.sh {da.Username}", () => {
						var toPost = downloadedData;
						if (toPost.ContentType == "image/gif") {
							switch (MessageBox.Show(this, "GIF images on DeviantArt require a separate preview image, which isn't possible via the API. Would you like to upload this image in PNG format instead?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
								case DialogResult.Cancel:
									return;
								case DialogResult.Yes:
									toPost = new PngRendition(toPost);
									break;
							}
						}

						using var f = new Form();

						f.Width = 600;
						f.Height = 350;
						var d = new DeviantArtUploadControl(da) {
							Dock = DockStyle.Fill
						};
						f.Controls.Add(d);
						d.Uploaded += url => f.Close();

						d.SetSubmission(ExportAsText(), toPost, null);

						f.ShowDialog(this);
					}));
					listBox1.Items.Add(new DestinationOption($"DeviantArt status update ({da.Username})", () => {
						using var f = new DeviantArtStatusUpdateForm(da, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var fa in settings.FurAffinity) {
					listBox1.Items.Add(new DestinationOption($"Fur Affinity ({fa.username})", () => {
						using var f = new FurAffinityPostForm(fa, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var fn in settings.FurryNetwork) {
					listBox1.Items.Add(new DestinationOption($"Furry Network ({fn.characterName})", () => {
						using var f = new FurryNetworkPostForm(fn, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var i in settings.Inkbunny) {
					listBox1.Items.Add(new DestinationOption($"Inkbunny ({i.username})", () => {
						using var f = new InkbunnyPostForm(i, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var m in settings.Pixelfed) {
					listBox1.Items.Add(new DestinationOption($"{m.AppRegistration.Instance} ({m.Username})", () => {
						using var f = new MastodonCwPostForm(m, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var m in settings.Pleronet) {
					listBox1.Items.Add(new DestinationOption($"{m.AppRegistration.Instance} ({m.Username})", () => {
						using var f = new MastodonCwPostForm(m, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var t in settings.Twitter) {
					listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
						using var f = new TwitterPostForm(t, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var t in settings.Tumblr) {
					listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
						using var f = new TumblrPostForm(t, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				foreach (var w in settings.WeasylApi) {
					if (w.apiKey == null) continue;
					listBox1.Items.Add(new DestinationOption($"Weasyl ({w.username})", () => {
						using var f = new WeasylPostForm(w, ExportAsText(), downloadedData);
						f.ShowDialog(this);
					}));
				}
				listBox1.Items.Add("");
			}

			listBox1.Items.Add("--- Post as text ---");

			foreach (var da in settings.DeviantArtTokens) {
				listBox1.Items.Add(new DestinationOption($"DeviantArt status update ({da.Username})", () => {
					using var f = new DeviantArtStatusUpdateForm(da, ExportAsText());
					f.ShowDialog(this);
				}));
			}
			foreach (var m in settings.Pixelfed) {
				listBox1.Items.Add(new DestinationOption($"{m.AppRegistration.Instance} ({m.Username})", () => {
					using var f = new MastodonCwPostForm(m, ExportAsText());
					f.ShowDialog(this);
				}));
			}
			foreach (var m in settings.Pleronet) {
				listBox1.Items.Add(new DestinationOption($"{m.AppRegistration.Instance} ({m.Username})", () => {
					using var f = new MastodonCwPostForm(m, ExportAsText());
					f.ShowDialog(this);
				}));
			}
			foreach (var t in settings.Twitter) {
				listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
					using var f = new TwitterPostForm(t, ExportAsText());
					f.ShowDialog(this);
				}));
			}
			foreach (var t in settings.Tumblr) {
				listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
					using var f = new TumblrPostForm(t, ExportAsText(), downloadedData);
					f.ShowDialog(this);
				}));
			}
		}

		private void btnPost_Click(object sender, EventArgs ea) {
			var o = listBox1.SelectedItem as DestinationOption;
			o?.Click?.Invoke();
		}

		public const string OpenFilter = "All supported formats|*.png;*.jpg;*.jpeg;*.gif;*.cps|Image files|*.png;*.jpg;*.jpeg;*.gif|CrosspostSharp JSON|*.cps|All files|*.*";

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			using var openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = OpenFilter;
			openFileDialog.Multiselect = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				LoadImage(openFileDialog.FileName);
			}
		}

		private void exportAsToolStripMenuItem_Click(object sender, EventArgs e) {
			if (_exportable == null) {
				MessageBox.Show(this, "This post does not have image data.", Text);
				return;
			}

			using var saveFileDialog = new SaveFileDialog();
			using (var ms = new MemoryStream(_exportable.Data, false))
			using (var image = Image.FromStream(ms)) {
				saveFileDialog.Filter = image.RawFormat.Equals(ImageFormat.Png) ? "PNG images|*.png"
					: image.RawFormat.Equals(ImageFormat.Jpeg) ? "JPEG images|*.jpg;*.jpeg"
					: image.RawFormat.Equals(ImageFormat.Gif) ? "GIF images|*.gif"
					: "All files|*.*";
			}
			if (saveFileDialog.ShowDialog() == DialogResult.OK) {
				File.WriteAllBytes(saveFileDialog.FileName, _exportable.Data);
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

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using var f = new AboutForm();
			f.ShowDialog(this);
		}

		private void mainWindowAccountSetupToolStripMenuItem_Click(object sender, EventArgs e) {
			using var f = new MainForm();
			f.ShowDialog(this);
		}
	}
}
