using CrosspostSharp3.Imgur;
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
		private readonly SavedPhotoPost _artworkData;

		public PillowfortPostForm(Settings.PillowfortSettings s, IPostBase post) {
			InitializeComponent();
			_client = new PillowfortClient { Cookie = s.cookie };

			foreach (var x in Enum.GetValues(typeof(PrivacyLevel))) {
				ddlPrivacy.Items.Add((PrivacyLevel)x);
			}
			ddlPrivacy.SelectedItem = PrivacyLevel.Public;

			_artworkData = post as SavedPhotoPost;
			txtTitle.Text = post.Title;
			txtDescription.Text = post.HTMLDescription;
			txtTags.Text = string.Join(", ", post.Tags);
			chkIncludeImage.Enabled = chkMakeSquare.Enabled = _artworkData != null;
			chkIncludeImage.Checked = _artworkData != null;
			chkNsfw.Checked = post.Mature || post.Adult;
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkMakeSquare.Enabled = chkIncludeImage.Checked;
		}

		private async void PillowfortPostForm_Shown(object sender, EventArgs e) {
			try {
				if (_artworkData != null) {
					using (var ms = new MemoryStream(_artworkData.data, false))
					using (var image = Image.FromStream(ms)) {
						chkMakeSquare.Checked = image.Height > image.Width;
					}
				}
			} catch (Exception) { }

			try {
				lblUsername1.Text = await _client.WhoamiAsync();

				var req = WebRequestFactory.Create(await _client.GetAvatarAsync());
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
			if (!(ddlPrivacy.SelectedItem is PrivacyLevel privacy)) {
				MessageBox.Show(this, "You must select a visibility level.");
				return;
			}

			string imageUrl = null;
			if (_artworkData != null && chkIncludeImage.Checked) {
				byte[] data = _artworkData.data;

				if (chkMakeSquare.Checked) {
					data = ImageUtils.MakeSquare(data);
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
