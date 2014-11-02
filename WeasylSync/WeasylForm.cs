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
	public partial class WeasylForm : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];

		// Stores references to the four WeasylThumbnail controls along the side. Each of them is responsible for fetching the submission information and image.
		private WeasylThumbnail[] thumbnails;

		// The current submission's details and image, which are fetched by the WeasylThumbnail and passed to SetCurrentImage.
		private SubmissionDetail currentSubmission;
		private byte[] currentImage;

		// Used for paging.
		private int? backid, nextid;

		// Allows WeasylThumbnail access to the progress bar.
		// Functions that use the progress bar should lock on it first.
		public LProgressBar LProgressBar {
			get {
				return lProgressBar1;
			}
		}

		// This function is called after clicking on a WeasylThumbnail.
		// It needs to be run on the GUI thread - WeasylThumbnail handles this using Invoke.
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

		public WeasylForm() {
			InitializeComponent();
			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };

			// Global tags that you can include in each submission if you want.
			// In the future these will be in app.config.
			txtTags2.Text = "#weasyl #" + USERNAME;

			backid = nextid = null;
			UpdateGalleryAsync();
		}

		// Launches a thread to update the thumbnails.
		// Progress is posted back to the LProgressBar, which handles its own thread safety using BeginInvoke.
		private Task UpdateGalleryAsync(int? backid = null, int? nextid = null) {
			Task t = new Task(() => {
				lock (lProgressBar1) {
					try {
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
					} catch (WebException ex) {
						lProgressBar1.Visible = false;
						MessageBox.Show(ex.Message);
					}
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

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Enabled = pickTime.Enabled = !chkNow.Checked;
		}
	}
}
