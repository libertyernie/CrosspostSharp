using ArtworkSourceSpecification;
using Pleronet.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace CrosspostSharp3.Mastodon {
	public partial class MastodonCwPostForm : Form {
		private readonly Settings.PleronetSettings _s;
		private readonly HttpClient _httpClient;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public MastodonCwPostForm(Settings.PleronetSettings s, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_s = s;

			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _s.Auth.AccessToken);

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

		private record PixelfedCollection(string Id, string Title);

		private async void MastodonCwPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _s.GetClient().GetCurrentUser();
				lblUsername1.Text = user.DisplayName;
				lblUsername2.Text = "@" + user.UserName + "@" + _s.AppRegistration.Instance;

				picUserIcon.ImageLocation = user.AvatarUrl;

				var collections = await _httpClient.GetFromJsonAsync<PixelfedCollection[]>($"https://{_s.AppRegistration.Instance}/api/local/profile/collections/{Uri.EscapeDataString(user.Id)}");
				foreach (var c in collections)
					listBox1.Items.Add(c);
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
				IEnumerable<(string, string)> getParameters() {
					yield return ("status", txtContent.Text);
					if (chkImageSensitive.Checked)
						yield return ("sensitive", "true");
					if (chkContentWarning.Checked)
						yield return ("spoiler_text", txtContentWarning.Text);
					foreach (var a in attachments)
						yield return ("media_ids[]", a.Id);
					foreach (PixelfedCollection c in listBox1.SelectedItems)
						yield return ("collection_ids[]", c.Id);
				}
				using var form = new FormUrlEncodedContent(getParameters().Select(x => new KeyValuePair<string, string>(x.Item1, x.Item2)));
				using var resp = await _httpClient.PostAsync($"https://{_s.AppRegistration.Instance}/api/v1/statuses", form);
				resp.EnsureSuccessStatusCode();
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
