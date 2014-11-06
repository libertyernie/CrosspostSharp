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
		private static Dictionary<string, Color> colors = new Dictionary<string, Color>() {
			{"general", SystemColors.WindowText},
			{"moderate", Color.FromArgb(32, 171, 230)},
			{"mature", Color.FromArgb(170, 187, 34)},
			{"explicit", Color.FromArgb(185, 30, 35)}
		};

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

					Console.WriteLine(Submission.rating);
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

		protected override void OnPaint(PaintEventArgs pe) {
			base.OnPaint(pe);

			Color c;
			if (Submission != null && colors.TryGetValue(Submission.rating, out c)) {
				using (Pen b = new Pen(c, 2)) {
					pe.Graphics.DrawRectangle(b, 1, 1, Width-2, Height-2);
				}
			}
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
