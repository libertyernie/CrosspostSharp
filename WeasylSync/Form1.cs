using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LWeasyl;

namespace WeasylSync {
	public partial class Form1 : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];

		private WeasylThumbnail[] thumbnails;
		private WeasylThumbnail current;
		private byte[] currentImage;
		private int? backid, nextid;

		public LProgressBar LProgressBar {
			get {
				return lProgressBar1;
			}
		}

		public void setCurrentImage(WeasylThumbnail thumbnail) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action<WeasylThumbnail>(setCurrentImage), thumbnail);
				return;
			}

			this.current = thumbnail;
			if (current.Details != null) {
				txtTitle.Text = current.Details.title;
				txtDescription.Text = current.Details.description;
				lblLink.Text = current.Details.link;
				txtTags.Text = string.Join(" ", current.Details.tags.Select(s => "#" + s));
				chkWeasylSubmitIdTag.Text = "#weasyl" + current.Details.submitid;
				pickDate.Value = pickTime.Value = current.Details.posted_at;
			}
			if (current.RawData == null) {
				mainPictureBox.Image = null;
			} else {
				Image image = null;
				try {
					image = Bitmap.FromStream(new MemoryStream(current.RawData));
					mainPictureBox.Image = image;
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
			}
		}

		public Form1() {
			InitializeComponent();
			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };

			backid = nextid = null;

			UpdateGalleryAsync();
		}

		private Task UpdateGalleryAsync(int? backid = null, int? nextid = null) {
			Task t = new Task(() => {
				lock (lProgressBar1) {
					lProgressBar1.Maximum = 8;
					lProgressBar1.Value = 0;
					lProgressBar1.Visible = true;
					var g = APIInterface.UserGallery(user: USERNAME, count: this.thumbnails.Length, backid: backid, nextid: nextid);
					lProgressBar1.Value += 4;
					for (int i = 0; i < this.thumbnails.Length; i++) {
						if (i < g.submissions.Length) {
							this.thumbnails[i].Submission = g.submissions[i];
						} else {
							this.thumbnails[i].Submission = null;
						}
						lProgressBar1.Value++;
					}
					this.backid = g.backid;
					this.nextid = g.nextid;
					this.BeginInvoke(new Action(() => {
						btnUp.Enabled = (this.backid != null);
						btnDown.Enabled = (this.nextid != null);
					}));
					lProgressBar1.Visible = false;
				}
			});
			t.Start();
			return t;
		}

		private void btnUp_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(backid: this.backid);
		}

		private void btnDown_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(nextid: this.nextid);
		}

		private void chkTitleBold_CheckedChanged(object sender, EventArgs e) {
			txtTitleSize_TextChanged(sender, e);
		}

		private void txtTitleSize_TextChanged(object sender, EventArgs e) {
			txtTitle.Font = new Font(txtTitle.Font.FontFamily, txtTitle.Font.Size, chkTitleBold.Checked ? FontStyle.Bold : FontStyle.Regular);
		}

		private void btnEmail_Click(object sender, EventArgs e) {
			StringBuilder sb = new StringBuilder();
			if (chkTitle.Checked) {
				List<string> styles = new List<string>();
				if (chkTitleBold.Checked) sb.Append("**");
				sb.Append(txtTitle.Text);
				if (chkTitleBold.Checked) sb.Append("**");
				sb.Append("\n\n");
			}
			if (chkDescription.Checked) {
				sb.Append(txtDescription.Text);
				sb.Append("\n\n");
			}
			if (chkLink.Checked) {
				sb.Append("[" + txtLink.Text + "](lblLink.Text)");
				sb.Append("\n\n");
			}
			if (chkWeasylTag.Checked) {
				sb.Append("#weasyl ");
			}
			if (chkTags.Checked) {
				sb.Append(txtTags.Text);
			}
			System.IO.File.WriteAllText("C:/Users/Owner/Desktop/dump.html", sb.ToString());
			System.Diagnostics.Process.Start("C:/Users/Owner/Desktop/dump.html");
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Enabled = pickTime.Enabled = !chkNow.Checked;
		}
	}
}
