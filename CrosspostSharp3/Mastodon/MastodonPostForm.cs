using Mastonet;
using Mastonet.Entities;
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
	public partial class MastodonPostForm : Form {
		private readonly MastodonClient _client;
		private readonly SavedPhotoPost _artworkData;
		private string _plainTextDescription;

		public MastodonPostForm(Settings.MastodonSettings s, SavedPhotoPost d) {
			InitializeComponent();
			_client = s.CreateClient();
			_artworkData = d;

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

			UpdateText();
		}

		private async void TwitterPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _client.GetCurrentUser();
				lblUsername1.Text = user.DisplayName;
				lblUsername2.Text = "@" + user.UserName + "@" + _client.Instance;

				picUserIcon.ImageLocation = user.AvatarUrl;
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
				sb.Append(_plainTextDescription);
			}

			if (chkIncludeLink.Checked) {
				sb.Append(' ');
				sb.Append(_artworkData.url);
			}
			textBox1.Text = sb.ToString();
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			try {
				Attachment a;
				using (var ms = new MemoryStream(_artworkData.data, false)) {
					a = await _client.UploadMedia(ms, PostConverter.CreateFilename(_artworkData));
				}
				await _client.PostStatus(textBox1.Text, Visibility.Public, mediaIds: new[] { a.Id }, sensitive: chkPotentiallySensitive.Checked);
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
