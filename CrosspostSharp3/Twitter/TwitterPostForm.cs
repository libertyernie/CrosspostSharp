using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3.Twitter {
	public partial class TwitterPostForm : Form {
		private readonly TwitterClient _credentials;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public TwitterPostForm(Settings.TwitterSettings s, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_credentials = s.GetCredentials();
			lblUsername2.Text = "@" + s.screenName;

			_post = post;
			_downloaded = downloaded;

			if (_downloaded != null) {
				chkIncludeImage.Enabled = true;
				chkIncludeImage.Checked = true;
				if (_downloaded.ContentType.StartsWith("video/")) {
					chkIncludeImage.Text = "Include video";
				}
			} else {
				chkIncludeImage.Enabled = false;
				chkIncludeImage.Checked = false;
			}

			txtContent.Text = HtmlConversion.ConvertHtmlToText(post.HTMLDescription);

			if (post.Mature || post.Adult) {
				chkPotentiallySensitive.Checked = true;
			}
		}

		private async void TwitterPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _credentials.Users.GetAuthenticatedUserAsync();
				lblUsername1.Text = user.Name;
				lblUsername2.Text = "@" + user.ScreenName;

				var req = WebRequest.Create(user.ProfileImageUrl);
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					picUserIcon.Image = Image.FromStream(ms);
				}
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			try {
				var attachments = new List<IMedia>();
				if (chkIncludeImage.Checked && _downloaded != null) {
					if (_downloaded.ContentType.StartsWith("video/")) {
						attachments.Add(await _credentials.Upload.UploadTweetVideoAsync(_downloaded.Data));
					} else {
						attachments.Add(await _credentials.Upload.UploadBinaryAsync(_downloaded.Data));
					}
				}

				var tweet = await _credentials.Tweets.PublishTweetAsync(new Tweetinvi.Parameters.PublishTweetParameters(txtContent.Text) {
					PossiblySensitive = chkPotentiallySensitive.Checked,
					Medias = attachments
				});
				if (tweet == null) {
					MessageBox.Show(this, "Could not post tweet");
				}
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			int count = txtContent.Text.Where(c => !char.IsLowSurrogate(c)).Count();
			lblCounter.Text = $"{count}/280";
		}
	}
}
