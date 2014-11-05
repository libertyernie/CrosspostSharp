using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using LWeasyl;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;

namespace WeasylSync {
	public partial class WeasylForm : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];

		public WeasylAPI Weasyl { get; private set; }

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
				txtURL.Text = submission.link;
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

			Weasyl = new WeasylAPI() { APIKey = ConfigurationManager.AppSettings["weasyl-api-key"] };
			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };

			// Global tags that you can include in each submission if you want.
			txtTags2.Text = ConfigurationManager.AppSettings["global-tags"] ?? "#weasyl";

			backid = nextid = null;
			UpdateGalleryAsync();
		}

		// Launches a thread to update the thumbnails.
		// Progress is posted back to the LProgressBar, which handles its own thread safety using BeginInvoke.
		private Task UpdateGalleryAsync(int? backid = null, int? nextid = null) {
			Task t = new Task(() => {
				lock (lProgressBar1) {
					try {
						lProgressBar1.Maximum = 4 + thumbnails.Length;
						lProgressBar1.Value = 0;
						lProgressBar1.Visible = true;
						var g = Weasyl.UserGallery(user: USERNAME, count: this.thumbnails.Length, backid: backid, nextid: nextid);
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
			txtTitle.Font = new Font(txtTitle.Font.FontFamily, txtTitle.Font.Size, chkTitleBold.Checked ? FontStyle.Bold : FontStyle.Regular);
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Visible = pickTime.Visible = !chkNow.Checked;
		}

		private void btnPost_Click(object sender, EventArgs e) {
			Token token = TumblrKey.Load();
			if (token == null) {
				token = TumblrKey.Obtain(OAuthConsumer.CONSUMER_KEY, OAuthConsumer.CONSUMER_SECRET);
				if (token == null) {
					MessageBox.Show("Posting cancelled.");
					return;
				} else {
					TumblrKey.Save(token);
				}
			}
			TumblrClient c = new TumblrClientFactory().Create<TumblrClient>(
				OAuthConsumer.CONSUMER_KEY,
				OAuthConsumer.CONSUMER_SECRET,
				token);
			c.CreatePostAsync(ConfigurationManager.AppSettings["tumblr-blog"], PostData.CreateText("Test 7, should not work also")).ContinueWith(new Action<Task<PostCreationInfo>>((t) => {
				Console.WriteLine("http://libertyernie.tumblr.com/post/" + t.Result.PostId);
			}));
		}

		private void chkTitle_CheckedChanged(object sender, EventArgs e) {
			txtTitle.Enabled = chkTitle.Checked;
		}

		private void chkDescription_CheckedChanged(object sender, EventArgs e) {
			txtDescription.Enabled = chkDescription.Checked;
		}

		private void chkFooter_CheckedChanged(object sender, EventArgs e) {
			txtFooter.Enabled = chkURL.Enabled = chkFooter.Checked;
			txtURL.Enabled = chkFooter.Checked && chkURL.Checked;
		}

		private void chkURL_CheckedChanged(object sender, EventArgs e) {
			txtFooter.Font = new Font(txtFooter.Font.FontFamily, txtFooter.Font.Size, chkURL.Checked ? FontStyle.Underline : FontStyle.Regular);
			txtFooter.ForeColor = chkURL.Checked ? Color.Blue : SystemColors.WindowText;
			txtURL.Enabled = chkFooter.Checked && chkURL.Checked;
		}

		private void chkTags1_CheckedChanged(object sender, EventArgs e) {
			txtTags1.Enabled = chkTags1.Checked;
		}

		private void chkTags2_CheckedChanged(object sender, EventArgs e) {
			txtTags2.Enabled = chkTags2.Checked;
		}
	}
}
