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
	public partial class TwitterPostForm : Form {
		private readonly ITwitterCredentials _credentials;
		private readonly SavedPhotoPost _artworkData;
		private string _plainTextDescription;

		private static int? _tcoUrlLength;

		public TwitterPostForm(Settings.TwitterSettings s, SavedPhotoPost d) {
			InitializeComponent();
			_credentials = s.GetCredentials();
			_artworkData = d;
			lblUsername2.Text = "@" + s.screenName;

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

			if (_tcoUrlLength == null) {
				_tcoUrlLength = Auth.ExecuteOperationWithCredentials(_credentials, () => {
					var conf = Tweetinvi.Help.GetTwitterConfiguration();
					return conf.ShortURLLengthHttps;
				});
			}

			UpdateText();
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
				if (_plainTextDescription == null) {
					try {
						_plainTextDescription = HtmlConversion.ConvertHtmlToText(_artworkData.description);
					} catch (Exception) {
						_plainTextDescription = _artworkData.description;
					}
				}
				sb.Append(_plainTextDescription); // todo convert to plaintext
			}

			int available = 280;
			if (chkIncludeLink.Checked) {
				available -= _tcoUrlLength.Value + 1;
			}
			if (sb.Length > available) {
				sb.Remove(available - 1, sb.Length - available + 1);
				sb.Append('…');
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
						IMedia media = Upload.UploadBinary(_artworkData.data);
						var tweet = await TweetAsync.PublishTweet(textBox1.Text, new Tweetinvi.Parameters.PublishTweetOptionalParameters {
							PossiblySensitive = chkPotentiallySensitive.Checked,
							Medias = new List<IMedia> { media }
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
			if (_artworkData.url != null && textBox1.Text.Contains(_artworkData.url)) {
				count -= _artworkData.url.Length;
				count += _tcoUrlLength.Value;
			}
			lblCounter.Text = $"{count}/280";
		}
	}
}
