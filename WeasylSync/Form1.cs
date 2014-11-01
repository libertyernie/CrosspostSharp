using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LWeasyl;
using System.Net.Mail;
using System.Diagnostics;

namespace WeasylSync {
	public partial class Form1 : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];

		private WeasylThumbnail[] thumbnails;

		private byte[] currentImage;
		public byte[] CurrentImage {
			get {
				return currentImage;
			}
			set {
				currentImage = value;
				if (value == null) {
					mainPictureBox.Image = null;
				} else {
					Image image = null;
					try {
						image = Bitmap.FromStream(new MemoryStream(value));
						mainPictureBox.Image = image;
					} catch (ArgumentException) {
						MessageBox.Show("This submission is not an image file.");
						mainPictureBox.Image = null;
					}
				}
			}
		}

		private SubmissionDetail details;
		public SubmissionDetail Details {
			get {
				return details;
			}
			set {
				details = value;
				if (value != null) {
					txtTitle.Text = value.title;
					txtDescription.Text = value.description;
					lblLink.Text = value.link;
					txtTags.Text = string.Join(" ", value.tags.Select(s => "#" + s));
					chkWeasylSubmitIdTag.Text = "#weasyl" + value.submitid;
					pickDate.Value = pickTime.Value = value.posted_at;
				}
			}
		}

		private WebClient client;

		private int? backid, nextid;
		private float originalFontSize;

		public Form1() {
			InitializeComponent();
			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };
			client = new WebClient();
			originalFontSize = txtTitle.Font.SizeInPoints;

			backid = nextid = null;

			lProgressBar1.Visible = true;
			lProgressBar1.Value = 0;
			UpdateGalleryAsync();
		}

		delegate void incrementProgressBarDelegate(int value);
		public void incrementProgressBar(int value) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new incrementProgressBarDelegate(incrementProgressBar), value);
			} else {
				lProgressBar1.Visible = true;
				value += lProgressBar1.Value;
				lProgressBar1.Value = value;
				lblDiagnostic.Text = value.ToString();
			}
		}

		delegate void setPagingDelegate(int? backid, int? nextid);
		public void setPaging(int? backid, int? nextid) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new setPagingDelegate(setPaging), backid, nextid);
			} else {
				this.backid = backid;
				btnUp.Enabled = (backid != null);
				this.nextid = nextid;
				btnDown.Enabled = (nextid != null);

				lProgressBar1.Value = 0;
				lProgressBar1.Visible = false;
			}
		}

		public void PopulateThumbnails(Gallery gallery) {
			for (int i = 0; i < this.thumbnails.Length; i++) {
				incrementProgressBar(16);
				if (i < gallery.submissions.Length) {
					this.thumbnails[i].Submission = gallery.submissions[i];
				} else {
					this.thumbnails[i].Submission = null;
				}
				Console.WriteLine(gallery.submissions[i].title);
			}
		}

		private Task UpdateGalleryAsync(int? backid = null, int? nextid = null) {
			Task t = new Task(() => {
				incrementProgressBar(64);
				var g = APIInterface.UserGallery(USERNAME, count: this.thumbnails.Length, backid: backid, nextid: nextid);
				PopulateThumbnails(g);
				setPaging(g.backid, g.nextid);
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
