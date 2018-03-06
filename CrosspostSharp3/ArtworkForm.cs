using ArtSourceWrapper;
using DeviantArtControls;
using Newtonsoft.Json;
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
		private ISubmissionWrapper _originalWrapper;

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
			return new ArtworkData {
				data = _data,
				title = txtTitle.Text,
				description = txtDescription.Text,
				url = _originalWrapper?.ViewURL,
				tags = txtTags.Text.Split(' ').Where(s => s != ""),
				mature = chkPotentiallySensitiveMaterial.Checked,
			};
		}

		public ArtworkForm() {
			InitializeComponent();

			Settings settings = Settings.Load();
			if (settings.DeviantArt?.RefreshToken != null) {
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
							txtDescription.Text,
							txtTags.Text.Split(' ').Where(s => s != ""),
							chkPotentiallySensitiveMaterial.Checked,
							_originalWrapper?.ViewURL);
						f.ShowDialog(this);
					}
				}));
			}
			foreach (var t in settings.Twitter) {
				listBox1.Items.Add(new DestinationOption($"Twitter ({t.Username})", () => {
					using (var f = new TwitterPostForm(t, Export())) {
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

		public ArtworkForm(string filename) : this() {
			this.Shown += (o, e) => LoadImage(filename);
		}

		public ArtworkForm(ArtworkData artworkData) : this() {
			this.Shown += (o, e) => LoadImage(artworkData);
		}

		public ArtworkForm(ISubmissionWrapper wrapper) : this() {
			this.Shown += (o, e) => LoadImage(wrapper);
		}

		public void LoadImage(string filename) {
			LoadImage(ArtworkData.FromFile(filename));
		}

		public void LoadImage(ArtworkData artwork) {
			using (var ms = new MemoryStream(artwork.data, false)) {
				var image = Image.FromStream(ms);
				splitContainer1.Panel1.BackgroundImage = image;
				splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
				_data = ms.ToArray();
			}
			txtTitle.Text = artwork.title;
			txtDescription.Text = artwork.description;
			txtTags.Text = string.Join(" ", artwork.tags);
			chkPotentiallySensitiveMaterial.Checked = artwork.mature;
			_originalWrapper = null;
		}

		public async void LoadImage(ISubmissionWrapper wrapper) {
			try {
				LoadImage(await ArtworkData.DownloadAsync(wrapper));
				_originalWrapper = wrapper;
				btnDelete.Enabled = _originalWrapper is IDeletable;
				btnView.Enabled = true;
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
			var data = artwork;
			data.description = HtmlConversion.ConvertHtmlToText(artwork.description);
			string jsonFile = Path.GetTempFileName();
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(data));

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

		private void lnkPreview_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			using (var f = new Form()) {
				f.Text = "Post to DeviantArt";
				f.Width = 600;
				f.Height = 350;
				var w = new WebBrowser {
					Dock = DockStyle.Fill
				};
				f.Controls.Add(w);
				w.Navigate("about:blank");
				w.Document.Write(txtDescription.Text);
				f.ShowDialog(this);
			}
		}

		public const string OpenFilter = "All supported formats|*.png;*.jpg;*.jpeg;*.gif;*.json|Image files|*.png;*.jpg;*.jpeg;*.gif|CrosspostSharp JSON metadata|*.cps.json|All files|*.*";

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
					saveFileDialog.Filter = "CrosspostSharp JSON metadata|*.cps.json|All files|*.*";
				}
				if (saveFileDialog.ShowDialog() == DialogResult.OK) {
					File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Export()));
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
			if (_originalWrapper is IDeletable d) {
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
			if (_originalWrapper?.ViewURL != null) {
				System.Diagnostics.Process.Start(_originalWrapper.ViewURL);
			}
		}
	}
}
