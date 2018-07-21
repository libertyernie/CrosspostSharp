using SourceWrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class TwitterNoPhotoPostForm : Form {
		private readonly ITwitterCredentials _credentials;
		
		public TwitterNoPhotoPostForm(Settings.TwitterSettings s, IPostMetadata post) {
			InitializeComponent();
			_credentials = s.GetCredentials();
			lblUsername2.Text = "@" + s.screenName;
			
			chkPotentiallySensitive.Checked = post.Mature || post.Adult;
			
			textBox1.Text = HtmlConversion.ConvertHtmlToText(post.HTMLDescription);
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
				await Auth.ExecuteOperationWithCredentials(
					_credentials,
					async () => {
						var tweet = await TweetAsync.PublishTweet(textBox1.Text, new Tweetinvi.Parameters.PublishTweetOptionalParameters {
							PossiblySensitive = chkPotentiallySensitive.Checked
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
			int count = textBox1.Text.Where(c => !char.IsLowSurrogate(c)).Count();
			lblCounter.Text = $"{count}/280";
		}
	}
}
