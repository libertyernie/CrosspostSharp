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

namespace WeasylSync {
	public partial class WeasylForm : Form {
		private static Settings GlobalSettings;

		public WeasylAPI Weasyl { get; private set; }
		private TumblrClient Tumblr;

		// Stores references to the four WeasylThumbnail controls along the side. Each of them is responsible for fetching the submission information and image.
		private WeasylThumbnail[] thumbnails;

		// The current submission's details and image, which are fetched by the WeasylThumbnail and passed to SetCurrentImage.
		private SubmissionDetail currentSubmission;
		private byte[] currentImage;

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
			UpdateGalleryAsync();
		}

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
			lblWeasylStatus2.Text = user == null ? "not logged in" : user.login;
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

			// Global tags that you can include in each submission if you want.
			txtTags2.Text = GlobalSettings.Tumblr.Tags ?? "";

			txtFooter.Text = GlobalSettings.Tumblr.Footer ?? "";
		}

		// This function is called after clicking on a WeasylThumbnail.
		// It needs to be run on the GUI thread - WeasylThumbnail handles this using Invoke.
		public void SetCurrentImage(SubmissionDetail submission, byte[] image) {
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
			this.currentImage = image;
			if (image == null) {
				mainPictureBox.Image = null;
			} else {
				Image bitmap = null;
				try {
					bitmap = Bitmap.FromStream(new MemoryStream(image));
					mainPictureBox.Image = bitmap;
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
			}
		}

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

		// Launches a thread to update the thumbnails.
		// Progress is posted back to the LProgressBar, which handles its own thread safety using BeginInvoke.
		private Task UpdateGalleryAsync(int? backid = null, int? nextid = null) {
			Task t = new Task(() => {
				lock (lProgressBar1) {
					try {
						lProgressBar1.Maximum = 4 + thumbnails.Length;
						lProgressBar1.Value = 0;
						lProgressBar1.Visible = true;
						var g = Weasyl.UserGallery(user: GlobalSettings.Weasyl.Username, count: this.thumbnails.Length, backid: backid, nextid: nextid);
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

		private string CompileHTML() {
			Console.WriteLine("CompileHTML");
			StringBuilder html = new StringBuilder();

			if (chkTitle.Checked) {
				html.Append("<p>");
				if (chkTitleBold.Checked) html.Append("<b>");
				html.Append(WebUtility.HtmlEncode(txtTitle.Text));
				if (chkTitleBold.Checked) html.Append("</b>");
				html.Append("</p>");
			}

			if (chkDescription.Checked) {
				html.Append(txtDescription.Text);
			}

			if (chkFooter.Checked) {
				html.Append(txtFooter.Text.Replace("{URL}", txtURL.Text));
			}

			return html.ToString();
		}

		private void btnUp_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(backid: this.backid);
		}

		private void btnDown_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(nextid: this.nextid);
		}

		private void chkTitleBold_CheckedChanged(object sender, EventArgs e) {
			txtTitle.Font = new Font(txtTitle.Font.FontFamily, txtTitle.Font.Size, chkTitleBold.Checked ? FontStyle.Bold : FontStyle.Regular);
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Visible = pickTime.Visible = !chkNow.Checked;
		}

		private void btnPost_Click(object sender, EventArgs e) {
			if (Tumblr == null) CreateTumblrClient_GetNewToken();
			if (Tumblr == null) {
				MessageBox.Show("Posting cancelled.");
				return;
			}
			/*Tumblr.CreatePostAsync(GlobalSettings.Tumblr.BlogName, PostData.CreateText("Test 8")).ContinueWith(new Action<Task<PostCreationInfo>>((t) => {
				Console.WriteLine("http://libertyernie.tumblr.com/post/" + t.Result.PostId);
			}));*/

		}

		private void chkTitle_CheckedChanged(object sender, EventArgs e) {
			txtTitle.Enabled = chkTitle.Checked;
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
					bool refreshGallery = GlobalSettings.Weasyl.Username != dialog.Settings.Weasyl.Username;
					GlobalSettings = dialog.Settings;
					GlobalSettings.Save();
					LoadFromSettings();
					if (refreshGallery) UpdateGalleryAsync();
				}
			}
		}

		private void viewFolderToolStripMenuItem_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(".");
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
					DocumentText = HTML_PREVIEW.Replace("{HTML}", CompileHTML()),
					Dock = DockStyle.Fill
				});
			}
		}

		private void chkHTMLPreview_CheckedChanged(object sender, EventArgs e) {
			UpdateHTMLPreview();
		}
	}
}
