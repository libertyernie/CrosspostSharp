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

namespace CrosspostSharp3 {
	public partial class MastodonCwPostForm : Form {
		private readonly Settings.MastodonSettings _s;
		private readonly SavedPhotoPost _artworkData;
		private readonly string _mp4url;

		public MastodonCwPostForm(Settings.MastodonSettings s, IPostBase post, IPostBase original) {
			InitializeComponent();
			_s = s;

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

			if (original is MastodonPostWrapper m) {
				txtContentWarning.Text = m.SpoilerText;
			} else if (original is IRemoteVideoPost t) {
				if (chkIncludeImage.Enabled == false) {
					// No image - check for video
					chkIncludeImage.Enabled = true;
					chkIncludeImage.Checked = true;
					chkIncludeImage.Text = "Include video";
					_mp4url = t.VideoURL;
				}
			}
		}

		private void chkContentWarning_CheckedChanged(object sender, EventArgs e) {
			txtContentWarning.Enabled = chkContentWarning.Checked;
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkImageSensitive.Enabled = chkIncludeImage.Checked;
			txtImageDescription.Enabled = chkIncludeImage.Checked;
		}

		private async void MastodonCwPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await Mastodon.Api.Accounts.VerifyCredentials(_s.Instance, _s.accessToken);
				lblUsername1.Text = user.DisplayName;
				lblUsername2.Text = "@" + user.UserName + "@" + _s.Instance;

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
				byte[] attachment_data = null;
				if (chkIncludeImage.Checked) {
					if (_artworkData != null) {
						attachment_data = _artworkData.data;
					} else if (_mp4url != null) {
						var req = WebRequest.Create(_mp4url);
						using (var resp = await req.GetResponseAsync())
						using (var s = resp.GetResponseStream())
						using (var ms = new MemoryStream()) {
							await s.CopyToAsync(ms);
							attachment_data = ms.ToArray();
						}
					}
				}
				string desc = chkIncludeImage.Checked ? txtImageDescription.Text : "";
				var attachments = attachment_data == null
					? Enumerable.Empty<Mastodon.Model.Attachment>()
					: new[] {
						await Mastodon.Api.Media.Uploading(_s.Instance, _s.accessToken, attachment_data, desc)
					};
				await Mastodon.Api.Statuses.Posting(
					_s.Instance,
					_s.accessToken,
					txtContent.Text,
					sensitive: chkImageSensitive.Checked,
					spoiler_text: chkContentWarning.Checked ? txtContentWarning.Text : null,
					media_ids: attachments.Select(a => checked((int)a.Id)).ToArray());
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
