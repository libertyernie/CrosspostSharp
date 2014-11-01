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

		private Form1 mainForm;

		public WeasylThumbnail() : base() {
			this.Click += WeasylThumbnail_Click;
		}

		void WeasylThumbnail_Click(object sender, EventArgs e) {
			if (this.mainForm == null) {
				for (Control c = this.Parent; c != null; c = c.Parent) {
					if (c is Form1) {
						this.mainForm = (Form1)c;
						break;
					}
				}
			}
			if (Submission != null) {
				new Task(() => {
					lock (mainForm.LProgressBar) {
						if (RawData == null) {
							WebRequest req = WebRequest.Create(Submission.media.submission.First().url);
							WebResponse resp = req.GetResponse();
							var stream = resp.GetResponseStream();

							RawData = new byte[resp.ContentLength];
							mainForm.LProgressBar.Maximum = RawData.Length;
							mainForm.LProgressBar.Value = 0;
							mainForm.LProgressBar.Visible = true;

							int read = 0;
							while (read < RawData.Length) {
								read += stream.Read(RawData, read, RawData.Length - read);
								mainForm.LProgressBar.Value = read;
							}
						}

						if (Details == null) {
							Details = APIInterface.ViewSubmission(Submission);
						}

						mainForm.LProgressBar.Visible = false;
						mainForm.Invoke(new Action<SubmissionDetail, byte[]>(mainForm.SetCurrentImage), Details, RawData);
					}
				}).Start();
			}
		}
	}
}
