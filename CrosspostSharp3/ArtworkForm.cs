using CrosspostSharp3.DeviantArt;
using CrosspostSharp3.FurAffinity;
using CrosspostSharp3.Mastodon;
using CrosspostSharp3.Weasyl;
using Microsoft.FSharp.Collections;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class ArtworkForm : Form {
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

		public void LoadImage(string filename) {
			IDownloadedData _downloaded = new LocalPhotoPost(filename);

			var image = Image.FromFile(filename);
			pictureBox1.Image = image;

			wbrDescription.Navigate("about:blank");
			string html = "";
			wbrDescription.Document.Write($"<html><head></head><body>{html}</body></html>");
			wbrDescription.Document.Body.SetAttribute("contenteditable", "true");

			Settings settings = Settings.Load();

			{
				listBox1.Items.Add("--- Post as photo ---");

				foreach (var da in settings.DeviantArtTokens) {
					listBox1.Items.Add(new DestinationOption($"DeviantArt / Sta.sh {da.Username}", () => {
						var toPost = _downloaded;
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
						using var f = new DeviantArtStatusUpdateForm(da, ExportAsText(), _downloaded);
						f.ShowDialog(this);
					}));
				}
				foreach (var fa in settings.FurAffinity) {
					listBox1.Items.Add(new DestinationOption($"Fur Affinity ({fa.username})", () => {
						using var f = new FurAffinityPostForm(fa, ExportAsText(), _downloaded);
						f.ShowDialog(this);
					}));
				}
				foreach (var m in settings.Pixelfed) {
					listBox1.Items.Add(new DestinationOption($"{m.AppRegistration.Instance} ({m.Username})", () => {
						using var f = new MastodonCwPostForm(m, ExportAsText(), _downloaded);
						f.ShowDialog(this);
					}));
				}
				foreach (var m in settings.Pleronet) {
					listBox1.Items.Add(new DestinationOption($"{m.AppRegistration.Instance} ({m.Username})", () => {
						using var f = new MastodonCwPostForm(m, ExportAsText(), _downloaded);
						f.ShowDialog(this);
					}));
				}
				foreach (var w in settings.WeasylApi) {
					if (w.apiKey == null) continue;
					listBox1.Items.Add(new DestinationOption($"Weasyl ({w.username})", () => {
						using var f = new WeasylPostForm(w, ExportAsText(), _downloaded);
						f.ShowDialog(this);
					}));
				}
				listBox1.Items.Add("");
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
