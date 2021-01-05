using ArtworkSourceSpecification;
using Pleronet.Entities;
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

namespace CrosspostSharp3.Mastodon {
	public partial class MastodonCwPostForm : Form {
		private readonly Settings.PleronetSettings _s;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public MastodonCwPostForm(Settings.PleronetSettings s, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_s = s;

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
				chkImageSensitive.Checked = true;
				chkContentWarning.Checked = true;
			}
		}

		private void chkContentWarning_CheckedChanged(object sender, EventArgs e) {
			txtContentWarning.Enabled = chkContentWarning.Checked;
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkImageSensitive.Enabled = chkIncludeImage.Checked;
			txtImageDescription.Enabled = chkIncludeImage.Checked;
			chkFocalPoint.Enabled = chkIncludeImage.Checked;
		}

		private async void MastodonCwPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _s.GetClient().GetCurrentUser();
				lblUsername1.Text = user.DisplayName;
				lblUsername2.Text = "@" + user.UserName + "@" + _s.AppRegistration.Instance;

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
				AttachmentFocusData focus = null;
				if (chkIncludeImage.Checked && _downloaded != null) {
					attachment_data = _downloaded.Data;
					if (chkFocalPoint.Checked) {
						using (var ms = new MemoryStream(attachment_data, false))
						using (var image = Image.FromStream(ms))
						using (var f = new FocalPointForm(image)) {
							if (f.ShowDialog() == DialogResult.OK) {
								focus = new AttachmentFocusData {
									X = f.FocalPoint.Item1,
									Y = f.FocalPoint.Item2
								};
							} else {
								return;
							}
						}
					}
				}
				var attachments = attachment_data == null
					? Enumerable.Empty<Attachment>()
					: new[] {
						await _s.GetClient().UploadMedia(new MemoryStream(attachment_data, false), txtImageDescription.Text, focus: focus)
					};
				await _s.GetClient().PostStatus(
					txtContent.Text,
					sensitive: chkImageSensitive.Checked,
					spoilerText: chkContentWarning.Checked ? txtContentWarning.Text : null,
					mediaIds: attachments.Select(a => a.Id).ToArray());
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
