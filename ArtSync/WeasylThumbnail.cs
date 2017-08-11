using ArtSourceWrapper;
using DontPanic.TumblrSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync {
	public class WeasylThumbnail : PictureBox {
		#region Properties and variables
		// Sets the submission for this thumbnail to display.
		public ISubmissionWrapper Submission { get; private set; }

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

            if (Submission != null) {
                using (Pen b = new Pen(Submission?.BorderColor ?? SystemColors.WindowText, 2)) {
                    pe.Graphics.DrawRectangle(b, 1, 1, Width - 2, Height - 2);
                }
            }
		}

		// Downloads the thumbnail pointed to by the submission info.
		public async Task SetSubmission(ISubmissionWrapper w) {
            Submission = w;

            string url = Submission?.ThumbnailURL ?? Submission?.ImageURL;
            if (url == null) {
				this.Image = null;
			} else {
				WebRequest req = WebRequest.Create(url);
                using (Stream memoryStream = new MemoryStream()) {
                    using (WebResponse resp = await req.GetResponseAsync()) {
                        using (Stream stream = resp.GetResponseStream()) {
                            await stream.CopyToAsync(memoryStream);
                            memoryStream.Position = 0;
                        }
                    }
                    this.Image = Image.FromStream(memoryStream);
                    if (this.Image.Width > 120 || this.Image.Height > 120) {
                        double largerDimension = Math.Max(this.Image.Width, this.Image.Height);
                        double scale = 120.0 / largerDimension;
                        this.Image = new Bitmap(this.Image,
                            (int)Math.Round(scale * this.Image.Width),
                            (int)Math.Round(scale * this.Image.Height));
                    }
                }
			}

			this.RawData = null;
		}

		// Downloads the submission details (including comments) and the submission image. Can be run on a separate thread.
		private async void FetchDetails() {
			try {
				if (RawData == null) {
                    mainForm.LProgressBar.Report(0);
					mainForm.LProgressBar.Visible = true;

					string url = Submission?.ImageURL;
                    if (url != null) {
                        string filename = url.Substring(url.LastIndexOf('/') + 1);

                        WebRequest req = WebRequest.Create(url);
                        WebResponse resp = await req.GetResponseAsync();
                        var stream = resp.GetResponseStream();

                        byte[] data;
                        if (resp.ContentLength == -1) {
                            // simple method, no progress bar
                            using (var ms = new MemoryStream()) {
                                await stream.CopyToAsync(ms);
                                data = ms.ToArray();
                            }
                        } else {
                            data = new byte[resp.ContentLength];

                            int read = 0;
                            while (read < data.Length) {
                                read += await stream.ReadAsync(data, read, data.Length - read);
                                mainForm.LProgressBar.Report(1.0 * read / data.Length);
                            }
                        }

                        RawData = new BinaryFile(data, filename, resp.ContentType);
                    }
				}

				mainForm.LProgressBar.Visible = false;
                
				await mainForm.SetCurrentImage(Submission, RawData);
			} catch (Exception ex) {
				mainForm.LProgressBar.Visible = false;
				MessageBox.Show(mainForm, ex.Message, ex.GetType().Name);
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
