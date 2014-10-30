using LWeasyl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeasylSync {
	public class WeasylThumbnail : PictureBox {
		private Submission _submission;
		public Submission Submission {
			get {
				return _submission;
			}
			set {
				_submission = value;

				if (value == null) {
					this.Image = null;
				} else {
					WebRequest req = WebRequest.Create(value.media.thumbnail.First().url);
					WebResponse resp = req.GetResponse();
					this.Image = Bitmap.FromStream(resp.GetResponseStream());
				}

				this.RawData = null;
				this.Details = null;
			}
		}

		public byte[] RawData { get; private set; }
		public SubmissionDetail Details { get; private set; }

		public WeasylThumbnail() : base() {
			this.Click += WeasylThumbnail_Click;
		}

		void WeasylThumbnail_Click(object sender, EventArgs e) {
			if (Submission != null) {
				if (RawData == null) {
					WebRequest req = WebRequest.Create(Submission.media.submission.First().url);
					WebResponse resp = req.GetResponse();
					var stream = resp.GetResponseStream();
					RawData = new byte[resp.ContentLength];
					int read = 0;
					while (read < RawData.Length) Console.WriteLine(read += stream.Read(RawData, read, RawData.Length - read));
				}
				if (Details == null) {
					Details = APIInterface.ViewSubmission(Submission);
				}
				Image image = null;
				try {
					image = Bitmap.FromStream(new MemoryStream(RawData));
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
				}

				for (Control c = this.Parent; c != null; c = c.Parent) {
					if (c is Form1) {
						Form1 mainForm = (Form1)c;

						mainForm.mainPictureBox.Image = image;
						mainForm.txtTitle.Text = Details.title;
						mainForm.txtDescription.Text = Details.description;
						mainForm.lblLink.Text = Details.link;
						mainForm.txtTags.Text = string.Join(" ", Details.tags.Select(s => "#" + s));

						mainForm.CurrentImage = RawData;

						break;
					}
				}
			}
		}
	}
}
