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
using WeasylReadOnly;

namespace WeasylView {
	public partial class Form1 : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];

		private int? backid, nextid;
		private PictureBox[] thumbnails;
		private Submission[] submissions;

		private WebClient client;
		private Image[] imageCache;

		private Image GetImage(string url) {
			try {
				byte[] data = client.DownloadData(url);
				return Bitmap.FromStream(new MemoryStream(data));
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
				MessageBox.Show(e.Message);
				return null;
			}
		}

		public Form1() {
			InitializeComponent();
			thumbnails = new PictureBox[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };
			submissions = new Submission[4];
			imageCache = new Image[4];
			client = new WebClient();

			for (int i=0; i<4; i++) {
				int j = i;
				thumbnails[j].Click += (o, e) => {
					if (submissions[j] != null) {
						imageCache[j] = mainPictureBox.Image = imageCache[j] ?? GetImage(submissions[j].media.submission.First().url);
					}
				};
			}

			backid = nextid = null;
			var gallery = APIInterface.UserGallery(USERNAME, count: 4);
			PopulateThumbnails(gallery.submissions);
		}

		public void PopulateThumbnails(Submission[] submissions) {
			for (int i=0; i<4; i++) {
				if (submissions.Length <= i) {
					this.thumbnails[i].Image = null;
					this.submissions[i] = null;
				} else {
					string url = submissions[i].media.thumbnail.First().url;
					byte[] data = client.DownloadData(url);
					this.thumbnails[i].Image = Bitmap.FromStream(new MemoryStream(data));
					this.submissions[i] = submissions[i];
				}
			}
		}
	}
}
