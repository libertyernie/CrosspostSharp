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

namespace CrosspostSharp3 {
	public partial class MastodonCwPostForm : Form {
		private readonly MastodonClient _client;
		private readonly SavedPhotoPost _artworkData;

		public MastodonCwPostForm(Settings.MastodonSettings s, IPostBase post, MastodonPostWrapper original) {
			InitializeComponent();
			_client = s.CreateClient();

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
				chkImageSensitive.Checked = true;
				chkContentWarning.Checked = true;
			}

			if (original != null) {
				txtContentWarning.Text = original.SpoilerText;
			}
		}

		private void chkContentWarning_CheckedChanged(object sender, EventArgs e) {
			txtContentWarning.Enabled = chkContentWarning.Checked;
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkImageSensitive.Enabled = chkIncludeImage.Checked;
		}

		private async void MastodonCwPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _client.GetCurrentUser();
				lblUsername1.Text = user.DisplayName;
				lblUsername2.Text = "@" + user.UserName + "@" + _client.Instance;

				picUserIcon.ImageLocation = user.AvatarUrl;
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			if (chkContentWarning.Checked && txtContentWarning.Text == "") {
				MessageBox.Show(this, "An empty content warning is not allowed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (chkContentWarning.Checked && chkIncludeImage.Checked && !chkImageSensitive.Checked) {
				MessageBox.Show(this, "You cannot use a content warning without also marking the image as sentitive.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try {
				var attachments = Enumerable.Empty<Attachment>();
				if (chkIncludeImage.Checked && _artworkData != null) {
					using (var ms = new MemoryStream(_artworkData.data, false)) {
						attachments = new[] {
							await _client.UploadMedia(ms, PostConverter.CreateFilename(_artworkData))
						};
					}
				}
				await _client.PostStatus(
					txtContent.Text,
					Visibility.Public,
					mediaIds: attachments.Select(a => a.Id),
					sensitive: chkImageSensitive.Checked,
					spoilerText: chkContentWarning.Checked ? txtContentWarning.Text : null);
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
