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
		private readonly SavedPhotoPost _artworkData;
		private string _mp4url;

		public TwitterPostForm(Settings.TwitterSettings s, IPostBase post, IPostBase original = null) {
			InitializeComponent();
			_credentials = s.GetCredentials();
			lblUsername2.Text = "@" + s.screenName;

			txtContent.Text = HtmlConversion.ConvertHtmlToText(post.HTMLDescription);

			if (post is SavedPhotoPost p) {
				_artworkData = p;
				chkIncludeImage.Enabled = true;
				chkIncludeImage.Checked = true;
			} else {
				chkIncludeImage.Checked = false;
				chkIncludeImage.Enabled = false;
			}

			if (post.Mature || post.Adult) {
				chkPotentiallySensitive.Checked = true;
			}

			if (original is IRemoteVideoPost t) {
				if (chkIncludeImage.Enabled == false) {
					// No image - check for video
					chkIncludeImage.Enabled = true;
					chkIncludeImage.Checked = true;
					chkIncludeImage.Text = "Include video";
					_mp4url = t.VideoURL;
				}
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
				if (chkIncludeImage.Checked) {
					if (_artworkData != null) {
						attachments.Add(
							Auth.ExecuteOperationWithCredentials(
								_credentials,
								() => Upload.UploadBinary(_artworkData.data)));
					} else if (_mp4url != null) {
						var req = WebRequest.Create(_mp4url);
						using (var resp = await req.GetResponseAsync())
						using (var s = resp.GetResponseStream())
						using (var ms = new MemoryStream()) {
							await s.CopyToAsync(ms);
							attachments.Add(
							Auth.ExecuteOperationWithCredentials(
								_credentials,
								() => Upload.UploadVideo(ms.ToArray())));
						}
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
