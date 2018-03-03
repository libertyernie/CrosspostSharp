using ArtSourceWrapper;
using DeviantArtControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
		private string _originalUrl;

		public ArtworkForm() {
			InitializeComponent();
		}

		public ArtworkForm(byte[] data) : this() {
			this.Shown += (o, e) => LoadImage(data);
		}

		public ArtworkForm(ISubmissionWrapper wrapper) : this() {
			this.Shown += (o, e) => LoadImage(wrapper);
		}

		public void LoadImage(byte[] data) {
			_data = data.ToArray();
			using (var ms = new MemoryStream(_data, false)) {
				var image = Image.FromStream(ms);
				splitContainer1.Panel1.BackgroundImage = image;
				splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
			}
			txtTitle.Text = "";
			txtDescription.Text = "";
			txtTags.Text = "";
			_originalUrl = null;
		}

		public async void LoadImage(ISubmissionWrapper wrapper) {
			try {
				var req = WebRequestFactory.Create(wrapper.ImageURL);
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					LoadImage(ms.ToArray());
				}
				txtTitle.Text = wrapper.Title;
				txtDescription.Text = wrapper.HTMLDescription;
				txtTags.Text = string.Join(" ", wrapper.Tags);
				_originalUrl = wrapper.ViewURL;
			} catch (Exception ex) {
				splitContainer1.Panel1.Controls.Add(new TextBox {
					Text = ex.Message + Environment.NewLine + ex.StackTrace,
					Multiline = true,
					Dock = DockStyle.Fill,
					ReadOnly = true
				});
			}
		}

		private void btnPost_Click(object sender, EventArgs e) {
			using (var f = new Form()) {
				f.Width = 600;
				f.Height = 350;
				var d = new DeviantArtUploadControl {
					Dock = DockStyle.Fill
				};
				f.Controls.Add(d);
				d.Uploaded += url => f.Close();
				d.SetSubmission(_data, txtTitle.Text, txtDescription.Text, txtTags.Text.Split(' '), chkPotentiallySensitiveMaterial.Checked, _originalUrl);
				f.ShowDialog(this);
			}
		}

		private void lnkPreview_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			using (var f = new Form()) {
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

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var openFileDialog = new OpenFileDialog()) {
				openFileDialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.gif";
				openFileDialog.Multiselect = false;
				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					LoadImage(File.ReadAllBytes(openFileDialog.FileName));
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
	}
}
