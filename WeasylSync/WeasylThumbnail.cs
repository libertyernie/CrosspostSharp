using DontPanic.TumblrSharp;
using WeasylSyncLib;
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

		#region Properties and variables
		// Sets the submission for this thumbnail to display.
		private SubmissionBaseDetail _submission;
		public SubmissionBaseDetail Submission {
			get {
				return _submission;
			}
			set {
				_submission = value;
				FetchSubmission();
			}
		}

		// This variable acts as a cache; it will be null until the thumbnail is clicked on.
		public BinaryFile RawData { get; private set; }

		// Reference to the parent form
		private WeasylForm mainForm;
		#endregion

		public WeasylThumbnail() : base() {
			this.Click += WeasylThumbnail_Click;
		}

		protected override void OnPaint(PaintEventArgs pe) {
			base.OnPaint(pe);

			Color c;
			if (Submission != null && colors.TryGetValue(Submission.rating, out c)) {
				using (Pen b = new Pen(c, 2)) {
					pe.Graphics.DrawRectangle(b, 1, 1, Width - 2, Height - 2);
				}
			}
		}

		// Downloads the thumbnail pointed to by the submission info.
		public void FetchSubmission() {
			if (Submission == null) {
				this.Image = null;
			} else {
				WebRequest req = WebRequest.Create(Submission.media.thumbnail.First().url);
				WebResponse resp = req.GetResponse();
				this.Image = Bitmap.FromStream(resp.GetResponseStream());
				if (this.Image.Width > 120 || this.Image.Height > 120) {
					double largerDimension = Math.Max(this.Image.Width, this.Image.Height);
					double scale = 120.0 / largerDimension;
					this.Image = new Bitmap(this.Image,
						(int)Math.Round(scale * this.Image.Width),
						(int)Math.Round(scale * this.Image.Height));
				}
			}

			this.RawData = null;
		}

		// Downloads the submission details (including comments) and the submission image. Can be run on a separate thread.
		private async Task FetchDetails() {
			try {
				if (RawData == null) {
					mainForm.LProgressBar.Value = 0;
					mainForm.LProgressBar.Visible = true;

					string url = Submission.media.submission.First().url;
					string filename = url.Substring(url.LastIndexOf('/') + 1);

					WebRequest req = WebRequest.Create(url);
					WebResponse resp = await req.GetResponseAsync();
					var stream = resp.GetResponseStream();

					byte[] data = new byte[resp.ContentLength];
					mainForm.LProgressBar.Maximum = data.Length;

					int read = 0;
					while (read < data.Length) {
						read += await stream.ReadAsync(data, read, data.Length - read);
						mainForm.LProgressBar.Value = read;
					}

					RawData = new BinaryFile(data, filename, resp.ContentType);
				}

				mainForm.LProgressBar.Visible = false;

				// SetCurrentImage needs to be run on the main thread, but we want to maintain the lock on the progress bar until it is complete.
				mainForm.Invoke(new Action<SubmissionBaseDetail, BinaryFile>(mainForm.SetCurrentImage), Submission, RawData);
			} catch (WebException ex) {
				mainForm.LProgressBar.Visible = false;
				MessageBox.Show(ex.Message);
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
				FetchDetails();
			}
		}
	}
}
