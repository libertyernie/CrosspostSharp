using ArtSourceWrapper;
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
	public partial class TwitterPostForm : Form {
		private readonly ITwitterCredentials _credentials;
		private readonly ArtworkForm.ArtworkData _artworkData;

		public TwitterPostForm(Settings.TwitterSettings s, ArtworkForm.ArtworkData d) {
			InitializeComponent();
			_credentials = s.GetCredentials();
			_artworkData = d;
			lblUsername2.Text = "@" + s.Username;

			UpdateText();

			if (string.IsNullOrEmpty(_artworkData.title)) {
				chkIncludeTitle.Enabled = false;
				chkIncludeTitle.Checked = false;
			}
			if (string.IsNullOrEmpty(_artworkData.description)) {
				chkIncludeDescription.Enabled = false;
				chkIncludeDescription.Checked = false;
			}
			if (string.IsNullOrEmpty(_artworkData.url)) {
				chkIncludeLink.Enabled = false;
				chkIncludeLink.Checked = false;
			}
			chkPotentiallySensitive.Checked = _artworkData.mature;

			chkIncludeTitle.CheckedChanged += (o, e) => UpdateText();
			chkIncludeDescription.CheckedChanged += (o, e) => UpdateText();
			chkIncludeLink.CheckedChanged += (o, e) => UpdateText();
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

		private void UpdateText() {
			StringBuilder sb = new StringBuilder();
			if (chkIncludeTitle.Checked) {
				sb.Append(_artworkData.title);
			}
			if (chkIncludeTitle.Checked && chkIncludeDescription.Checked) {
				sb.Append("﹘");
			}
			if (chkIncludeDescription.Checked) {
				sb.Append(_artworkData.description); // todo convert to plaintext
			}
			if (chkIncludeLink.Checked) {
				sb.Append(' ');
				sb.Append(_artworkData.url);
			}
			textBox1.Text = sb.ToString();
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			try {
				await Auth.ExecuteOperationWithCredentials(
					_credentials,
					async () => {
						IMedia media = Upload.UploadImage(_artworkData.data);
						await TweetAsync.PublishTweet(textBox1.Text, new Tweetinvi.Parameters.PublishTweetOptionalParameters {
							PossiblySensitive = chkPotentiallySensitive.Checked,
							Medias = new List<IMedia> { media }
						});
					});
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
