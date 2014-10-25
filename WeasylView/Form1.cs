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

namespace WeasylView {
	public partial class Form1 : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];
		public static readonly int TS = 4;

		private int? backid, nextid;
		private PictureBox[] thumbnails;
		private Submission[] submissions;

		private WebClient client;
		private Tuple<Image, SubmissionDetail>[] imageCache;

		private float originalFontSize;

		private Image GetImage(string url) {
			try {
				byte[] data = client.DownloadData(url);
				return Bitmap.FromStream(new MemoryStream(data));
			} catch (ArgumentException) {
				MessageBox.Show("This submission is not an image file.");
				return null;
			}
		}

		public Form1() {
			InitializeComponent();
			thumbnails = new PictureBox[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };
			submissions = new Submission[TS];
			imageCache = new Tuple<Image, SubmissionDetail>[TS];
			client = new WebClient();
			originalFontSize = txtTitle.Font.SizeInPoints;

			for (int i = 0; i < TS; i++) {
				int j = i;
				thumbnails[j].Click += (o, e) => {
					if (submissions[j] != null) {
						if (imageCache[j] == null) {
							var image = GetImage(submissions[j].media.submission.First().url);
							var detail = APIInterface.ViewSubmission(submissions[j]);
							imageCache[j] = new Tuple<Image, SubmissionDetail>(image, detail);
						}
						mainPictureBox.Image = imageCache[j].Item1;
						txtTitle.Text = imageCache[j].Item2.title;
						txtDescription.Text = imageCache[j].Item2.description;
						txtTags.Text = string.Join(" ", imageCache[j].Item2.tags.Select(s => "#" + s));
					}
				};
			}

			backid = nextid = null;
			var gallery = APIInterface.UserGallery(USERNAME, count: TS);
			PopulateThumbnails(gallery);
		}

		public void PopulateThumbnails(Gallery gallery) {
			imageCache = new Tuple<Image, SubmissionDetail>[TS];
			for (int i = 0; i < TS; i++) {
				if (gallery.submissions.Length <= i) {
					this.thumbnails[i].Image = null;
					this.submissions[i] = null;
				} else {
					this.thumbnails[i].Image = GetImage(gallery.submissions[i].media.thumbnail.First().url);
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
			float i;
			float size = float.TryParse(txtTitleSize.Text, out i) ? i : originalFontSize;
			txtTitle.Font = new Font(txtTitle.Font.FontFamily, size, chkTitleBold.Checked ? FontStyle.Bold : FontStyle.Regular);
		}
	}
}
