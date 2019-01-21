using SourceWrappers;
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

namespace CrosspostSharp3 {
	public partial class TwitterPostForm : Form {
		private readonly ITwitterCredentials _credentials;
		private readonly IPostBase _post;
		private readonly IDownloadedData _downloaded;

		public TwitterPostForm(Settings.TwitterSettings s, IPostBase post, IDownloadedData downloaded = null) {
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
				var user = await Auth.ExecuteOperationWithCredentials(
					_credentials,
					async () => await UserAsync.GetAuthenticatedUser());
				lblUsername1.Text = user.Name;
				lblUsername2.Text = "@" + user.ScreenName;

				var req = WebRequestFactory.Create(user.ProfileImageUrl);
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
						attachments.Add(
							Auth.ExecuteOperationWithCredentials(
								_credentials,
								() => Upload.UploadVideo(_downloaded.Data)));
					} else {
						attachments.Add(
							Auth.ExecuteOperationWithCredentials(
								_credentials,
								() => Upload.UploadBinary(_downloaded.Data)));
					}
				}

				await Auth.ExecuteOperationWithCredentials(
					_credentials,
					async () => {
						var tweet = await TweetAsync.PublishTweet(txtContent.Text, new Tweetinvi.Parameters.PublishTweetOptionalParameters {
							PossiblySensitive = chkPotentiallySensitive.Checked,
							Medias = attachments
						});
						if (tweet == null) {
							MessageBox.Show(this, "Could not post tweet");
						}
					});
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
