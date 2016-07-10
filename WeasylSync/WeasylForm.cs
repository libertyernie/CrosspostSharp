using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeasylSyncLib;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using InkbunnyLib;

namespace WeasylSync {
	public partial class WeasylForm : Form {
		private static Settings GlobalSettings;

		public WeasylAPI Weasyl { get; private set; }
		public string WeasylUsername { get; private set; }
		public string WeasylExceptionMsg { get; private set; }

		private TumblrClient Tumblr;
		public string TumblrUsername { get; private set; }
		public string TumblrExceptionMsg { get; private set; }

		public InkbunnyClient Inkbunny;

		// Stores references to the four WeasylThumbnail controls along the side. Each of them is responsible for fetching the submission information and image.
		private WeasylThumbnail[] thumbnails;

		// The current submission's details and image, which are fetched by the WeasylThumbnail and passed to SetCurrentImage.
		private SubmissionDetail currentSubmission;
		private BinaryFile currentImage;

		// The image displayed in the main panel. This is used again if WeasylSync needs to add padding to the image to force a square aspect ratio.
		private Bitmap currentImageBitmap;

		// Used for paging.
		private int? backid, nextid;

		// The existing Tumblr post for the selected Weasyl submission, if any - looked up by using the #weasylXXXXXX tag.
		private BasePost ExistingPost;

		// Allows WeasylThumbnail access to the progress bar.
		// Functions that use the progress bar should lock on it first.
		public LProgressBar LProgressBar {
			get {
				return lProgressBar1;
			}
		}

		public WeasylForm() {
			InitializeComponent();

			GlobalSettings = Settings.Load();

			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };

			backid = nextid = null;

            this.Height -= grpInkbunny.Height;
            grpInkbunny.Visible = false;

			new Task(LoadFromSettings).Start();
		}

		#region GUI updates
		private void LoadFromSettings() {
			lock (LProgressBar) {
				LProgressBar.Value = 0;
				LProgressBar.Maximum = 2;
				LProgressBar.Visible = true;

				Weasyl = new WeasylAPI() { APIKey = GlobalSettings.Weasyl.APIKey };

				Token token = GlobalSettings.TumblrToken;
				if (token != null && token.IsValid) {
					if (Tumblr != null) Tumblr.Dispose();
					Tumblr = new TumblrClientFactory().Create<TumblrClient>(
						OAuthConsumer.CONSUMER_KEY,
						OAuthConsumer.CONSUMER_SECRET,
						token);
				}

				User user = null;
				WeasylExceptionMsg = null;
				try {
					user = Weasyl.Whoami();
				} catch (WebException e) {
					if (e.Response is HttpWebResponse) {
						WeasylExceptionMsg = ((HttpWebResponse)e.Response).StatusDescription;
					} else {
						WeasylExceptionMsg = e.Message;
					}
				}
				bool refreshGallery = user == null || WeasylUsername != user.login;
				WeasylUsername = user == null ? null : user.login;

				LProgressBar.Value = 1;

				TumblrExceptionMsg = null;
				if (Tumblr == null) {
					lblTumblrStatus2.Text = "not logged in";
					lblTumblrStatus2.ForeColor = SystemColors.WindowText;
				} else {
					try {
						var t = Tumblr.GetUserInfoAsync();
						t.Wait();
						TumblrUsername = t.Result.Name;
					} catch (AggregateException e) {
						TumblrUsername = null;
						TumblrExceptionMsg = e.InnerException.Message;
					}
				}

				LProgressBar.Visible = false;
				if (this.IsHandleCreated) this.BeginInvoke(new Action<bool>(UpdateSettingsInWindow), refreshGallery);
			}
		}

		private void UpdateSettingsInWindow(bool refreshGallery) {
			lblWeasylStatus2.Text = WeasylUsername ?? WeasylExceptionMsg ?? "not logged in";
			lblWeasylStatus2.ForeColor = WeasylUsername == null ? SystemColors.WindowText : Color.DarkGreen;

			lblTumblrStatus2.Text = TumblrUsername ?? TumblrExceptionMsg ?? "not logged in";
			lblTumblrStatus2.ForeColor = TumblrUsername == null ? SystemColors.WindowText : Color.DarkGreen;

			txtHeader.Text = GlobalSettings.Defaults.HeaderHTML ?? "";
			txtFooter.Text = GlobalSettings.Defaults.FooterHTML ?? "";
			// Global tags that you can include in each submission if you want.
			txtTags2.Text = GlobalSettings.Defaults.Tags ?? "";

			chkWeasylSubmitIdTag.Checked = GlobalSettings.Defaults.IncludeWeasylTag;

			if (refreshGallery) UpdateGalleryAsync();
		}

		// This function is called after clicking on a WeasylThumbnail.
		// It needs to be run on the GUI thread - WeasylThumbnail handles this using Invoke.
		public void SetCurrentImage(SubmissionDetail submission, BinaryFile file) {
			this.currentSubmission = submission;
			if (submission != null) {
				txtTitle.Text = submission.title;
				txtDescription.Text = submission.GetDescription(true);
                txtInkbunnyDescription.Text = HtmlToBBCode.ConvertHtml(txtDescription.Text);
				txtURL.Text = submission.link;
				txtTags1.Text = string.Join(" ", submission.tags.Select(s => "#" + s));
				chkWeasylSubmitIdTag.Text = "#weasyl" + submission.submitid;
				pickDate.Value = pickTime.Value = submission.posted_at;
				UpdateHTMLPreview();
			}
			this.currentImage = file;
			if (file == null) {
				mainPictureBox.Image = null;
			} else {
				try {
					this.currentImageBitmap = (Bitmap)Bitmap.FromStream(new MemoryStream(file.Data));
					mainPictureBox.Image = this.currentImageBitmap;
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
			}
			UpdateExistingPostLink();
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
						if (WeasylUsername != null) {
							var g = Weasyl.UserGallery(user: WeasylUsername, count: this.thumbnails.Length, backid: backid, nextid: nextid);
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
						} else {
							lProgressBar1.Value += 8;
							for (int i = 0; i < this.thumbnails.Length; i++) {
								this.thumbnails[i].Submission = null;
							}
							this.backid = null;
							this.nextid = null;
						}
						if (this.IsHandleCreated) this.BeginInvoke(new Action(() => {
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
		#endregion

		#region Tumblr lookup
		private void UpdateExistingPostLink() {
			if (this.InvokeRequired) {
				if (this.IsHandleCreated) this.BeginInvoke(new Action(UpdateExistingPostLink));
				return;
			}

			if (!GlobalSettings.Tumblr.FindPreviousPost) {
				this.btnPost.Enabled = true;
				this.lblAlreadyPosted.Text = "";
				this.lnkTumblrPost.Text = "";
			} else if (Tumblr != null) {
				this.btnPost.Enabled = false;
				this.lblAlreadyPosted.Text = "Checking your Tumblr for tag " + chkWeasylSubmitIdTag.Text + "...";
				this.lnkTumblrPost.Text = "";
				this.GetTaggedPostsForSubmissionAsync().ContinueWith((t) => {
					this.ExistingPost = t.Result.Result.FirstOrDefault();
					if (this.ExistingPost != null) {
						SetCorresponsingPostUrl(this.ExistingPost.Url);
					} else {
						SetCorresponsingPostUrl(null);
					}
				});
			}
		}

		public void SetCorresponsingPostUrl(string url) {
			if (this.InvokeRequired) {
				this.Invoke(new Action<string>(SetCorresponsingPostUrl), url);
				return;
			}

			this.btnPost.Enabled = true;
			if (string.IsNullOrEmpty(url)) {
				this.lblAlreadyPosted.Text = "";
				this.lnkTumblrPost.Text = "";
			} else {
				this.lblAlreadyPosted.Text = "Already on Tumblr:";
				this.lnkTumblrPost.Text = url;
			}
		}
		#endregion

		#region HTML compilation
		public string CompileHTML() {
			StringBuilder html = new StringBuilder();

			if (chkHeader.Checked) {
				html.Append(txtHeader.Text);
			}

			if (chkDescription.Checked) {
				html.Append(txtDescription.Text);
			}

			if (chkFooter.Checked) {
				html.Append(txtFooter.Text);
			}

			html.Replace("{TITLE}", WebUtility.HtmlEncode(txtTitle.Text)).Replace("{URL}", txtURL.Text);

			return html.ToString();
		}

		private static string HTML_PREVIEW = @"
<html>
	<head>
	<style type='text/css'>
		body {
			font-family: ""Helvetica Neue"",""HelveticaNeue"",Helvetica,Arial,sans-serif;
			font-weight: 400;
			line-height: 1.4;
			font-size: 14px;
			font-style: normal;
			color: #444;
		}
		p {
			margin: 0 0 10px;
			padding: 0px;
			border: 0px none;
			font: inherit;
			vertical-align: baseline;
		}
		a img {
			border: 0;
		}
	</style>
</head>
	<body>{HTML}</body>
</html>";

		private void UpdateHTMLPreview() {
			previewPanel.Visible = chkHTMLPreview.Checked;
			previewPanel.Controls.Clear();
			if (chkHTMLPreview.Checked) {
				previewPanel.Controls.Add(new WebBrowser {
					DocumentText = HTML_PREVIEW.Replace("{HTML}", this.CompileHTML()),
					Dock = DockStyle.Fill
				});
			}
		}
		#endregion

		#region Tumblr
		private void CreateTumblrClient_GetNewToken() {
			Token token = TumblrKey.Obtain(OAuthConsumer.CONSUMER_KEY, OAuthConsumer.CONSUMER_SECRET);
			if (token == null) {
				return;
			} else {
				GlobalSettings.TumblrToken = token;
				GlobalSettings.Save();
				Tumblr = new TumblrClientFactory().Create<TumblrClient>(
					OAuthConsumer.CONSUMER_KEY,
					OAuthConsumer.CONSUMER_SECRET,
					token);
			}
		}

		private Task<Posts> GetTaggedPostsForSubmissionAsync() {
			string uniquetag = chkWeasylSubmitIdTag.Text.Replace("#", "");
			return Tumblr.GetPostsAsync(GlobalSettings.Tumblr.BlogName, 0, 1, PostType.All, false, false, PostFilter.Html, uniquetag);
		}

		private void PostToTumblr1() {
			if (this.currentImage == null) {
				MessageBox.Show("No image is selected.");
				return;
			}

			if (Tumblr == null) CreateTumblrClient_GetNewToken();
			if (Tumblr == null) {
				MessageBox.Show("Posting cancelled.");
				return;
			}

			lProgressBar1.Maximum = 2;
			lProgressBar1.Value = 0;
			lProgressBar1.Visible = true;

			lProgressBar1.Value = 1;

			long? updateid = null;
			if (this.ExistingPost!= null) {
				DialogResult result = new PostAlreadyExistsDialog(chkWeasylSubmitIdTag.Text, this.ExistingPost.Url).ShowDialog();
				if (result == DialogResult.Cancel) {
					lProgressBar1.Visible = false;
					return;
				} else if (result == PostAlreadyExistsDialog.Result.Replace) {
					updateid = this.ExistingPost.Id;
				}
			}

			lProgressBar1.Maximum = 2;
			lProgressBar1.Value = 2;
			lProgressBar1.Visible = true;

			var tags = new List<string>();
			if (chkTags1.Checked) tags.AddRange(txtTags1.Text.Replace("#", "").Split(' ').Where(s => s != ""));
			if (chkTags2.Checked) tags.AddRange(txtTags2.Text.Replace("#", "").Split(' ').Where(s => s != ""));
			if (chkWeasylSubmitIdTag.Checked) tags.AddRange(chkWeasylSubmitIdTag.Text.Replace("#", "").Split(' ').Where(s => s != ""));

			BinaryFile imageToPost = GlobalSettings.Tumblr.AutoSidePadding && this.currentImageBitmap.Height > this.currentImageBitmap.Width
				? MakeSquare(this.currentImageBitmap)
				: currentImage;

			PostData post = PostData.CreatePhoto(new BinaryFile[] { imageToPost }, CompileHTML(), txtURL.Text, tags);
			post.Date = chkNow.Checked
				? (DateTimeOffset?)null
				: (pickDate.Value.Date + pickTime.Value.TimeOfDay);

			Task<PostCreationInfo> task = updateid == null
				? Tumblr.CreatePostAsync(GlobalSettings.Tumblr.BlogName, post)
				: Tumblr.EditPostAsync(GlobalSettings.Tumblr.BlogName, updateid.Value, post);
			task.ContinueWith((t) => {
				lProgressBar1.Visible = false;
				if (t.Exception != null && t.Exception is AggregateException) {
					var messages = t.Exception.InnerExceptions.Select(x => x.Message);
					MessageBox.Show("An error occured: \"" + string.Join(", ", messages) + "\"\r\nCheck to see if the blog name is correct.");
				} else {
					UpdateExistingPostLink();
				}
			});
		}
		#endregion

		#region Inkbunny
		public void InkbunnyLogin() {
            using (LoginDialog d = new LoginDialog()) {
            	if (d.ShowDialog() == DialogResult.OK) {
					lblInkbunnyStatus2.Text = "Working...";
					Task.Run(() => {
						try {
							Inkbunny = new InkbunnyClient(d.Username, d.Password);
                            if (this.IsHandleCreated) this.BeginInvoke(new Action(() => {
                                this.Height += grpInkbunny.Height;
                                grpInkbunny.Visible = true;
                                lblInkbunnyStatus2.Text = Inkbunny.Username;
							}));
						} catch (Exception ex) {
							if (this.IsHandleCreated) this.BeginInvoke(new Action(() => {
								lblInkbunnyStatus2.Text = "click to log in";
							}));
							MessageBox.Show(ex.Message);
						}
					});
				}
			}
		}

		public async Task PostToInkbunny1() {
			if (this.currentImage == null) {
				MessageBox.Show("No image is selected.");
				return;
			}

			if (Inkbunny == null) {
				MessageBox.Show("You must log into Inkbunny before posting.");
                InkbunnyLogin();
                return;
			}

			lProgressBar1.Maximum = 2;
			lProgressBar1.Value = 1;
			lProgressBar1.Visible = true;

			lProgressBar1.Value = 1;

            try {
                long submission_id = await Inkbunny.Upload(files: new byte[][] {
                    currentImage.Data
                });

                if (this.IsHandleCreated) this.BeginInvoke(new Action(() => {
                    lProgressBar1.Value = 2;
                }));

                var o = await Inkbunny.EditSubmission(
                    submission_id: submission_id,
                    title: txtTitle.Text,
                    desc: txtInkbunnyDescription.Text,
                    convert_html_entities: true,
                    type: SubmissionType.Picture,
                    scraps: chkInkbunnyScraps.Checked,
                    isPublic: chkInkbunnyPublic.Checked,
                    notifyWatchersWhenPublic: chkInkbunnyNotifyWatchers.Checked,
                    keywords: txtTags1.Text.Replace("#", "").Split(' ').Where(s => s != ""),
                    tag: new InkbunnyRating() {
                        Nudity = chkInbunnyTag2.Checked,
                        Violence = chkInbunnyTag3.Checked,
                        SexualThemes = chkInbunnyTag4.Checked,
                        StrongViolence = chkInbunnyTag5.Checked,
                    }
                );
                Console.WriteLine(o.submission_id);
                Console.WriteLine(o.twitter_authentication_success);

                if (this.IsHandleCreated) this.BeginInvoke(new Action(() => {
                    lProgressBar1.Visible = false;
                }));
            } catch (Exception ex) {
                MessageBox.Show("An error occured: \"" + ex.Message + "\"\r\nCheck to see if the blog name is correct.");
            }
        }
		#endregion

		#region Event handlers
		private void btnUp_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(backid: this.backid);
		}

		private void btnDown_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(nextid: this.nextid);
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Visible = pickTime.Visible = !chkNow.Checked;
		}

		private void btnPost_Click(object sender, EventArgs args) {
			PostToTumblr1();
		}

		private void chkTitle_CheckedChanged(object sender, EventArgs e) {
			txtHeader.Enabled = chkHeader.Checked;
		}

		private void chkDescription_CheckedChanged(object sender, EventArgs e) {
			txtDescription.Enabled = chkDescription.Checked;
		}

		private void chkFooter_CheckedChanged(object sender, EventArgs e) {
			txtFooter.Enabled = chkFooter.Checked;
			txtURL.Enabled = chkFooter.Checked;
		}

		private void chkTags1_CheckedChanged(object sender, EventArgs e) {
			txtTags1.Enabled = chkTags1.Checked;
		}

		private void chkTags2_CheckedChanged(object sender, EventArgs e) {
			txtTags2.Enabled = chkTags2.Checked;
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs args) {
			using (SettingsDialog dialog = new SettingsDialog(GlobalSettings)) {
				if (dialog.ShowDialog() != DialogResult.Cancel) {
					GlobalSettings = dialog.Settings;
					GlobalSettings.Save();
					LoadFromSettings();
				}
			}
		}

		private void chkHTMLPreview_CheckedChanged(object sender, EventArgs e) {
			UpdateHTMLPreview();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var d = new AboutDialog()) d.ShowDialog(this);
		}

		private void lnkTumblrPost_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(lnkTumblrPost.Text);
        }

        private void lblInkbunnyStatus2_Click(object sender, EventArgs e) {
            InkbunnyLogin();
        }

        private void btnInkbunnyPost_Click(object sender, EventArgs e) {
            PostToInkbunny1();
        }

        private void chkInkbunnyPublic_CheckedChanged(object sender, EventArgs e) {
            chkInkbunnyNotifyWatchers.Enabled = chkInkbunnyPublic.Checked;
        }
        #endregion

        private static BinaryFile MakeSquare(Bitmap oldBitmap) {
			int newSize = Math.Max(oldBitmap.Width, oldBitmap.Height);
			Bitmap newBitmap = new Bitmap(newSize, newSize);

			int offsetX = (newSize - oldBitmap.Width) / 2;
			int offsetY = (newSize - oldBitmap.Height) / 2;

			using (Graphics g = Graphics.FromImage(newBitmap)) {
				g.DrawImage(oldBitmap, offsetX, offsetY, oldBitmap.Width, oldBitmap.Height);
			}

			using (MemoryStream stream = new MemoryStream()) {
				newBitmap.Save(stream, oldBitmap.RawFormat);
				return new BinaryFile(stream.ToArray());
			}
		}
	}
}
