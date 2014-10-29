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

namespace WeasylSync {
	public partial class Form1 : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];
		public static readonly int TS = 4;

		private PictureBox[] thumbnails;
		private Submission[] submissions;

		private WebClient client;
		private Tuple<byte[], SubmissionDetail>[] imageCache;

		private int? backid, nextid;
		private float originalFontSize;
		private byte[] currentImage;

		public Form1() {
			InitializeComponent();
			thumbnails = new PictureBox[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };
			submissions = new Submission[TS];
			imageCache = new Tuple<byte[], SubmissionDetail>[TS];
			client = new WebClient();
			originalFontSize = txtTitle.Font.SizeInPoints;

			for (int i = 0; i < TS; i++) {
				int j = i;
				thumbnails[j].Click += (o, e) => {
					if (submissions[j] != null) {
						if (imageCache[j] == null) {
							byte[] data = client.DownloadData(submissions[j].media.submission.First().url);
							var detail = APIInterface.ViewSubmission(submissions[j]);
							imageCache[j] = new Tuple<byte[], SubmissionDetail>(data, detail);
						}
						currentImage = imageCache[j].Item1;
						Image image = null;
						try {
							image = Bitmap.FromStream(new MemoryStream(currentImage));
						} catch (ArgumentException) {
							MessageBox.Show("This submission is not an image file.");
						}
						mainPictureBox.Image = image;
						txtTitle.Text = imageCache[j].Item2.title;
						txtDescription.Text = imageCache[j].Item2.description;
						lblLink.Text = imageCache[j].Item2.link;
						txtTags.Text = string.Join(" ", imageCache[j].Item2.tags.Select(s => "#" + s));
					}
				};
			}

			backid = nextid = null;
			Task<Gallery> t = new Task<Gallery>(() => {
				return APIInterface.UserGallery(USERNAME, count: TS);
			});
			t.Start();
			t.GetAwaiter().OnCompleted(new Action(() => {
				PopulateThumbnails(t.Result);
			}));
		}

		public void PopulateThumbnails(Gallery gallery) {
			imageCache = new Tuple<byte[], SubmissionDetail>[TS];
			for (int i = 0; i < TS; i++) {
				if (gallery.submissions.Length <= i) {
					this.thumbnails[i].Image = null;
					this.submissions[i] = null;
				} else {
					byte[] data = client.DownloadData(gallery.submissions[i].media.thumbnail.First().url);
					this.thumbnails[i].Image = Bitmap.FromStream(new MemoryStream(data));
					this.submissions[i] = gallery.submissions[i];
				}
			}
			this.backid = gallery.backid;
			btnUp.Enabled = (backid != null);
			this.nextid = gallery.nextid;
			btnDown.Enabled = (nextid != null);
		}

		private void btnUp_Click(object sender, EventArgs e) {
			PopulateThumbnails(APIInterface.UserGallery(USERNAME, count: TS, backid: this.backid));
		}

		private void btnDown_Click(object sender, EventArgs e) {
			PopulateThumbnails(APIInterface.UserGallery(USERNAME, count: TS, nextid: this.nextid));
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
	}
}
