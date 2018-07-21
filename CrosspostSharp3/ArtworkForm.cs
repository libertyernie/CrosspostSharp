using CrosspostSharp3.Weasyl;
using DeviantArtControls;
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
		private byte[] _data;
		private string _url;
		private IPostMetadata _originalWrapper;

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

		public ArtworkData Export() {
			return new ArtworkData(
				data: _data,
				title: txtTitle.Text,
				description: wbrDescription.Document.Body.InnerHtml,
				url: _url,
				tags: txtTags.Text.Split(' ').Where(s => s != ""),
				mature: chkMature.Checked,
				adult: chkAdult.Checked
			);
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

		public ArtworkForm(ArtworkData artworkData) : this() {
			this.Shown += (o, e) => LoadImage(artworkData);
		}

		public ArtworkForm(IPostWrapper wrapper) : this() {
			this.Shown += (o, e) => LoadImage(wrapper);
		}

		public void LoadImage(string filename) {
			LoadImage(PostConverter.FromFile(filename));
		}

		public void LoadImage(ArtworkData artwork) {
			using (var ms = new MemoryStream(artwork.data, false)) {
				var image = Image.FromStream(ms);
				splitContainer1.Panel1.BackgroundImage = image;
				splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
				_data = ms.ToArray();
			}

			txtTitle.Text = artwork.title;
			wbrDescription.Navigate("about:blank");
			wbrDescription.Document.Write($"<html><head></head><body>{artwork.description}</body></html>");
			wbrDescription.Document.Body.SetAttribute("contenteditable", "true");
			txtTags.Text = string.Join(" ", artwork.tags);
			chkMature.Checked = artwork.mature;
			chkAdult.Checked = artwork.adult;

			_url = artwork.url;
			btnView.Enabled = _url != null;

			_originalWrapper = null;

			BeginInvoke(new Action(ReloadOptions));
		}

		private void ReloadOptions() {
			Settings settings = Settings.Load();

			listBox1.Items.Add("--- Post as text ---");

			saveAsToolStripMenuItem.Enabled = false;
			exportAsToolStripMenuItem.Enabled = false;

			if (settings.DeviantArt.RefreshToken != null) {
				listBox1.Items.Add(new DestinationOption("DeviantArt status update", () => {
					using (var f = new DeviantArtStatusUpdateForm(wbrDescription.Document.Body.InnerHtml)) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var p in settings.Pillowfort) {
				listBox1.Items.Add(new DestinationOption($"Pillowfort ({p.username})", () => {
					using (var f = new PillowfortPostForm(p, wbrDescription.Document.Body.InnerHtml, _originalWrapper.Mature || _originalWrapper.Adult)) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Twitter) {
				listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
					string text = HtmlConversion.ConvertHtmlToText(wbrDescription.Document.Body.InnerHtml);
					using (var f = new TwitterNoPhotoPostForm(t, text, _originalWrapper.Mature || _originalWrapper.Adult)) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Tumblr) {
				listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
					using (var f = new TumblrNoPhotoPostForm(t, wbrDescription.Document.Body.InnerHtml)) {
						f.ShowDialog(this);
					}
				}));
			}

			listBox1.Items.Add("");
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
							_data,
							txtTitle.Text,
							wbrDescription.Document.Body.InnerHtml,
							txtTags.Text.Split(' ').Where(s => s != ""),
							chkMature.Checked || chkAdult.Checked,
							_url);
						f.ShowDialog(this);
					}
				}));
				listBox1.Items.Add(new DestinationOption("DeviantArt status update", () => {
					using (var f = new DeviantArtStatusUpdateForm(wbrDescription.Document.Body.InnerHtml, _data)) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var fl in settings.Flickr) {
				listBox1.Items.Add(new DestinationOption($"Flickr ({fl.username})", () => {
					using (var f = new FlickrPostForm(fl, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var fn in settings.FurryNetwork) {
				listBox1.Items.Add(new DestinationOption($"Furry Network ({fn.characterName})", () => {
					using (var f = new FurryNetworkPostForm(fn, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var i in settings.Inkbunny) {
				listBox1.Items.Add(new DestinationOption($"Inkbunny ({i.username})", () => {
					using (var f = new InkbunnyPostForm(i, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var p in settings.Pillowfort) {
				listBox1.Items.Add(new DestinationOption($"Pillowfort ({p.username})", () => {
					using (var f = new PillowfortPostForm(p, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Twitter) {
				listBox1.Items.Add(new DestinationOption($"Twitter ({t.screenName})", () => {
					using (var f = new TwitterPostForm(t, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Tumblr) {
				listBox1.Items.Add(new DestinationOption($"Tumblr ({t.blogName})", () => {
					using (var f = new TumblrPostForm(t, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var w in settings.Weasyl) {
				if (w.wzl == null) continue;
				listBox1.Items.Add(new DestinationOption($"Weasyl ({w.username})", () => {
					using (var f = new WeasylPostForm(w, Export())) {
						f.ShowDialog(this);
					}
				}));
			}
			if (File.Exists("efc.jar")) {
				listBox1.Items.Add(new DestinationOption($"FurAffinity / Weasyl", () => {
					LaunchEFC(Export());
				}));
			}
		}

		public async void LoadImage(IPostWrapper wrapper) {
			try {
				LoadImage(await PostConverter.DownloadAsync(wrapper));
				_originalWrapper = wrapper;
				btnDelete.Enabled = _originalWrapper is SourceWrappers.IDeletable;
			} catch (Exception ex) {
				splitContainer1.Panel1.Controls.Add(new TextBox {
					Text = ex.Message + Environment.NewLine + ex.StackTrace,
					Multiline = true,
					Dock = DockStyle.Fill,
					ReadOnly = true
				});
			}
		}

		private static void LaunchEFC(ArtworkData artwork) {
			string jsonFile = Path.GetTempFileName();
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(new {
				artwork.adult,
				artwork.data,
				description = HtmlConversion.ConvertHtmlToText(artwork.description),
				artwork.mature,
				artwork.tags,
				artwork.title,
				artwork.url
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
			using (var saveFileDialog = new SaveFileDialog()) {
				using (var ms = new MemoryStream(_data, false))
				using (var image = Image.FromStream(ms)) {
					saveFileDialog.Filter = "CrosspostSharp JSON|*.cps|All files|*.*";
				}
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Export(), Formatting.Indented));
				}
			}
		}

		private void exportAsToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var saveFileDialog = new SaveFileDialog()) {
				using (var ms = new MemoryStream(_data, false))
				using (var image = Image.FromStream(ms)) {
					saveFileDialog.Filter = image.RawFormat.Equals(ImageFormat.Png) ? "PNG images|*.png"
						: image.RawFormat.Equals(ImageFormat.Jpeg) ? "JPEG images|*.jpg;*.jpeg"
						: image.RawFormat.Equals(ImageFormat.Gif) ? "GIF images|*.gif"
						: "All files|*.*";
				}
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllBytes(saveFileDialog.FileName, _data);
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
			if (_originalWrapper is SourceWrappers.IDeletable d) {
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
			if (_url != null) {
				Process.Start(_url);
			}
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
