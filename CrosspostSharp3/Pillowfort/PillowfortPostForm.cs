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

		public PillowfortPostForm(Settings.PillowfortSettings s, ArtworkData artworkData) {
			InitializeComponent();
			_client = new PillowfortClient { Cookie = s.cookie };

			_artworkData = artworkData;
			txtTitle.Text = artworkData.title;
			txtDescription.Text = artworkData.description;
			txtTags.Text = string.Join(", ", artworkData.tags);
			chkIncludeImage.Enabled = true;
			chkIncludeImage.Checked = true;
			chkNsfw.Checked = artworkData.mature || artworkData.adult;
		}

		public PillowfortPostForm(Settings.PillowfortSettings s, string html, bool nsfw) {
			InitializeComponent();
			_client = new PillowfortClient { Cookie = s.cookie };
			
			txtDescription.Text = html;
			chkIncludeImage.Enabled = false;
			chkIncludeImage.Checked = false;
			chkNsfw.Checked = nsfw;
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
