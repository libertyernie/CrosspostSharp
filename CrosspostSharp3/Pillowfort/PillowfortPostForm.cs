using PillowfortFs;
using SourceWrappers;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class PillowfortPostForm : Form {
		private readonly PillowfortClient _client;
		private readonly ArtworkData _artworkData;

		public PillowfortPostForm(Settings.PillowfortSettings s, IPostMetadata post) {
			InitializeComponent();
			_client = new PillowfortClient { Cookie = s.cookie };

			_artworkData = post as ArtworkData;
			txtTitle.Text = post.Title;
			txtDescription.Text = post.HTMLDescription;
			txtTags.Text = string.Join(", ", post.Tags);
			chkIncludeImage.Enabled = _artworkData != null;
			chkIncludeImage.Checked = _artworkData != null;
			chkNsfw.Checked = post.Mature || post.Adult;
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkMakeSquare.Enabled = chkIncludeImage.Checked;
		}

		private async void PillowfortPostForm_Shown(object sender, EventArgs e) {
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
	}
}
