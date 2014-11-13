using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using LWeasyl;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace WeasylSync {
	public partial class WeasylForm : Form {
		private static Settings GlobalSettings;

		public WeasylAPI Weasyl { get; private set; }
		public string WeasylUsername { get; private set; }

		private TumblrClient Tumblr;

		// Stores references to the four WeasylThumbnail controls along the side. Each of them is responsible for fetching the submission information and image.
		private WeasylThumbnail[] thumbnails;

		// The current submission's details and image, which are fetched by the WeasylThumbnail and passed to SetCurrentImage.
		private SubmissionDetail currentSubmission;
		private BinaryFile currentImage;

		// The Tumblr post made from the current submission, if any; looked up by using the #weasyl{submitid} tag.
		private long? correlatedTumblrPostId;

		// Used for paging.
		private int? backid, nextid;

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

			LoadFromSettings();

			backid = nextid = null;
		}

		#region GUI updates
		private void LoadFromSettings() {
			Weasyl = new WeasylAPI() { APIKey = GlobalSettings.Weasyl.APIKey };

			Token token = GlobalSettings.TumblrToken;
			if (token != null && token.IsValid) {
				if (Tumblr != null) Tumblr.Dispose();
				Tumblr = new TumblrClientFactory().Create<TumblrClient>(
					OAuthConsumer.CONSUMER_KEY,
					OAuthConsumer.CONSUMER_SECRET,
					token);
			}

			User user = Weasyl.Whoami();
			bool refreshGallery = user == null || WeasylUsername != user.login;
			WeasylUsername = user == null ? null : user.login;
			lblWeasylStatus2.Text = WeasylUsername ?? "not logged in";
			lblWeasylStatus2.ForeColor = user == null ? SystemColors.WindowText : Color.DarkGreen;

			if (Tumblr == null) {
				lblTumblrStatus2.Text = "not logged in";
				lblTumblrStatus2.ForeColor = SystemColors.WindowText;
			} else {
				try {
					var t = Tumblr.GetUserInfoAsync();
					t.Wait();
					lblTumblrStatus2.Text = t.Result.Name;
					lblTumblrStatus2.ForeColor = Color.DarkGreen;
				} catch (AggregateException e) {
					lblTumblrStatus2.Text = string.Join(", ", e.InnerExceptions.Select(x => x.Message));
					lblTumblrStatus2.ForeColor = Color.DarkRed;
				}
			}

			txtHeader.Text = GlobalSettings.Tumblr.Header ?? "";
			txtFooter.Text = GlobalSettings.Tumblr.Footer ?? "";
			// Global tags that you can include in each submission if you want.
			txtTags2.Text = GlobalSettings.Tumblr.Tags ?? "";

			if (refreshGallery) UpdateGalleryAsync();
		}

		// This function is called after clicking on a WeasylThumbnail.
		// It needs to be run on the GUI thread - WeasylThumbnail handles this using Invoke.
		public void SetCurrentImage(SubmissionDetail submission, BinaryFile file) {
			this.currentSubmission = submission;
			if (submission != null) {
				txtTitle.Text = submission.title;
				txtDescription.Text = submission.description;
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
				Image bitmap = null;
				try {
					bitmap = Bitmap.FromStream(new MemoryStream(file.Data));
					mainPictureBox.Image = bitmap;
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
			}
			this.correlatedTumblrPostId = null;
			if (Tumblr != null) {
				this.GetTaggedPostsForSubmissionAsync().ContinueWith((t) => {
					if (t.Result.Result.Any()) {
						this.correlatedTumblrPostId = t.Result.Result.First().Id;
					}
				});
			}
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
						this.BeginInvoke(new Action(() => {
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

		private void PostToTumblr(bool forceNew = false) {
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

			if (forceNew == false) {
				this.GetTaggedPostsForSubmissionAsync().ContinueWith((t) => {
					lProgressBar1.Value = 1;
					if (!t.Result.Result.Any()) {
						PostToTumblr(true);
					} else if (new PostAlreadyExistsDialog(chkWeasylSubmitIdTag.Text, t.Result.Result.First().Url).ShowDialog() == DialogResult.OK) {
						PostToTumblr(true);
					} else {
						lProgressBar1.Visible = false;
					}
				});
				return;
			} else {
				lProgressBar1.Value = 2;

				var tags = new List<string>();
				if (chkTags1.Checked) tags.AddRange(txtTags1.Text.Replace("#", "").Split(' ').Where(s => s != ""));
				if (chkTags2.Checked) tags.AddRange(txtTags2.Text.Replace("#", "").Split(' ').Where(s => s != ""));
				if (chkWeasylSubmitIdTag.Checked) tags.AddRange(chkWeasylSubmitIdTag.Text.Replace("#", "").Split(' ').Where(s => s != ""));

				PostData post = PostData.CreatePhoto(new BinaryFile[] { this.currentImage }, CompileHTML(), txtURL.Text, tags);
				post.Date = chkNow.Checked
					? (DateTimeOffset?)null
					: (pickDate.Value.Date + pickTime.Value.TimeOfDay);

				Tumblr.CreatePostAsync(GlobalSettings.Tumblr.BlogName, post).ContinueWith((t) => {
				//Tumblr.EditPostAsync(GlobalSettings.Tumblr.BlogName, 102485978051, post).ContinueWith((t) => {
					lProgressBar1.Visible = false;
					if (t.Exception != null && t.Exception is AggregateException) {
						var messages = t.Exception.InnerExceptions.Select(x => x.Message);
						MessageBox.Show("An error occured: \"" + string.Join(", ", messages) + "\"\r\nCheck to see if the blog name is correct.");
					} else {
						correlatedTumblrPostId = t.Result.PostId;
					}
				});
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
			PostToTumblr();
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

		private void viewFolderToolStripMenuItem_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(".");
		}

		private void chkHTMLPreview_CheckedChanged(object sender, EventArgs e) {
			UpdateHTMLPreview();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var d = new AboutDialog()) d.ShowDialog(this);
		}

		private void btnViewExistingTumblrPost_Click(object sender, EventArgs e) {
			if (this.correlatedTumblrPostId != null && Tumblr != null) {
				Tumblr.GetPostAsync(this.correlatedTumblrPostId.Value).ContinueWith((t) => {
					System.Diagnostics.Process.Start(t.Result.Url);
				});
			}
		}
		#endregion
	}
}
