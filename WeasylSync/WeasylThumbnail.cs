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
		// Sets the submission for this thumbnail to display.
		// The Submission object comes from the Weasyl gallery; the thumbnail is fetched immediately; and the details and actual image are fetched when necessary.
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

		// These variables act as a cache; they will be null until the thumbnail is clicked on.
		public byte[] RawData { get; private set; }
		public SubmissionDetail Details { get; private set; }

		// Reference to the parent form
		private WeasylForm mainForm;

		public WeasylThumbnail() : base() {
			this.Click += WeasylThumbnail_Click;
		}

		void WeasylThumbnail_Click(object sender, EventArgs e) {
			// Search for the parent form if we don't already have a reference to it
			if (this.mainForm == null) {
				for (Control c = this.Parent; c != null; c = c.Parent) {
					if (c is WeasylForm) {
						this.mainForm = (WeasylForm)c;
						break;
					}
				}
			}
			if (Submission != null) {
				// Launch a new thread to download the submission details and raw image data
				new Task(() => {
					lock (mainForm.LProgressBar) {
						try {
							if (RawData == null) {
								mainForm.LProgressBar.Value = 0;
								mainForm.LProgressBar.Visible = true;

								WebRequest req = WebRequest.Create(Submission.media.submission.First().url);
								WebResponse resp = req.GetResponse();
								var stream = resp.GetResponseStream();

								RawData = new byte[resp.ContentLength];
								mainForm.LProgressBar.Maximum = RawData.Length;

								int read = 0;
								while (read < RawData.Length) {
									read += stream.Read(RawData, read, RawData.Length - read);
									mainForm.LProgressBar.Value = read;
								}
							}

							if (Details == null) {
								Details = mainForm.Weasyl.ViewSubmission(Submission);
							}

							mainForm.LProgressBar.Visible = false;

							// SetCurrentImage needs to be run on the main thread, but we want to maintain the lock on the progress bar until it is complete.
							mainForm.Invoke(new Action<SubmissionDetail, byte[]>(mainForm.SetCurrentImage), Details, RawData);
						} catch (WebException ex) {
							mainForm.LProgressBar.Visible = false;
							MessageBox.Show(ex.Message);
						}
					}
				}).Start();
			}
		}
	}
}
