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
		private SubmissionDetail currentSubmission;
		private byte[] currentImage;
		private int? backid, nextid;

		public LProgressBar LProgressBar {
			get {
				return lProgressBar1;
			}
		}

		public void SetCurrentImage(SubmissionDetail submission, byte[] image) {
			this.currentSubmission = submission;
			if (submission != null) {
				txtTitle.Text = submission.title;
				txtDescription.Text = submission.description;
				lblLink.Text = submission.link;
				txtTags1.Text = string.Join(" ", submission.tags.Select(s => "#" + s));
				chkWeasylSubmitIdTag.Text = "#weasyl" + submission.submitid;
				pickDate.Value = pickTime.Value = submission.posted_at;
			}
			this.currentImage = image;
			if (image == null) {
				mainPictureBox.Image = null;
			} else {
				Image bitmap = null;
				try {
					bitmap = Bitmap.FromStream(new MemoryStream(image));
					mainPictureBox.Image = bitmap;
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
			}
		}

		public Form1() {
			InitializeComponent();
			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };

			txtTags2.Text = "#weasyl #" + USERNAME;

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
			if (chkTags1.Checked) {
				sb.Append(txtTags1.Text);
			}
			sb.Append(" ");
			if (chkTags2.Checked) {
				sb.Append(txtTags2.Text);
			}
			System.IO.File.WriteAllText("C:/Users/Owner/Desktop/dump.html", sb.ToString());
			System.Diagnostics.Process.Start("C:/Users/Owner/Desktop/dump.html");
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Enabled = pickTime.Enabled = !chkNow.Checked;
		}
	}
}
