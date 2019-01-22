﻿using CrosspostSharp3.Imgur;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using PillowfortFs;
using SourceWrappers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class PillowfortPostForm : Form {
		private readonly PillowfortClient _client;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public PillowfortPostForm(Settings.PillowfortSettings s, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_client = new PillowfortClient { Cookie = s.cookie };

			foreach (var x in Enum.GetValues(typeof(PrivacyLevel))) {
				ddlPrivacy.Items.Add((PrivacyLevel)x);
			}
			ddlPrivacy.SelectedItem = PrivacyLevel.Public;

			_post = post;
			_downloaded = downloaded;
			txtTitle.Text = post.Title;
			txtDescription.Text = post.HTMLDescription;
			txtTags.Text = string.Join(", ", post.Tags);
			chkIncludeImage.Enabled = chkMakeSquare.Enabled = downloaded != null;
			chkIncludeImage.Checked = downloaded != null;
			chkNsfw.Checked = post.Mature || post.Adult;
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkMakeSquare.Enabled = chkIncludeImage.Checked;
		}

		private async void PillowfortPostForm_Shown(object sender, EventArgs e) {
			try {
				if (_downloaded != null) {
					using (var ms = new MemoryStream(_downloaded.Data, false))
					using (var image = Image.FromStream(ms)) {
						chkMakeSquare.Checked = image.Height > image.Width;
					}
				}
			} catch (Exception) { }

			try {
				lblUsername1.Text = await _client.WhoamiAsync();

				string avatar = await _client.GetAvatarAsync();
				if (avatar != null) {
					var req = WebRequestFactory.Create(avatar);
					using (var resp = await req.GetResponseAsync())
					using (var stream = resp.GetResponseStream())
					using (var ms = new MemoryStream()) {
						await stream.CopyToAsync(ms);
						ms.Position = 0;
						picUserIcon.Image = Image.FromStream(ms);
					}
				}
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			if (!(ddlPrivacy.SelectedItem is PrivacyLevel privacy)) {
				MessageBox.Show(this, "You must select a visibility level.");
				return;
			}

			string imageUrl = null;
			if (_downloaded != null && chkIncludeImage.Checked) {
				byte[] data = _downloaded.Data;

				if (chkMakeSquare.Checked) {
					data = ImageUtils.MakeSquare(data);
				}

				if (MessageBox.Show(this, "This will upload the image itself to Imgur. Is this OK?", Text, MessageBoxButtons.OKCancel) != DialogResult.OK) {
					return;
				}

				imageUrl = await ImgurAnonymousUpload.UploadAsync(data);
			}
			
			try {
				await _client.SubmitPostAsync(new PostRequest(
					title: txtTitle.Text,
					content: txtDescription.Text,
					tags: txtTags.Text.Replace("#", "").Split(',').Select(s => s.Trim()).Where(s => s != ""),
					privacy: privacy,
					rebloggable: chkRebloggable.Checked,
					commentable: chkCommentable.Checked,
					nsfw: chkNsfw.Checked,
					media: imageUrl != null
						? PillowfortMediaBuilder.Photo(imageUrl)
						: PillowfortMediaBuilder.None));
				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
