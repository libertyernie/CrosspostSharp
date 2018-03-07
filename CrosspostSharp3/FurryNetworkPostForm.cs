using ArtSourceWrapper;
using FurryNetworkLib;
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
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class FurryNetworkPostForm : Form {
		private readonly FurryNetworkClient _client;
		private readonly string _characterName;
		private readonly ArtworkData _artworkData;

		public FurryNetworkPostForm(Settings.FurryNetworkSettings s, ArtworkData d) {
			InitializeComponent();
			_client = new FurryNetworkClient(s.refreshToken);
			_artworkData = d;
			_characterName = s.characterName;
			lblUsername1.Text = s.characterName;

			txtTitle.Text = d.title;
			txtDescription.Enabled = false;
			txtTags.Text = string.Join(" ", d.tags);

			if (_artworkData.adult) {
				radFurryNetworkRating2.Checked = true;
			} else if (_artworkData.mature) {
				radFurryNetworkRating1.Checked = true;
			} else {
				radFurryNetworkRating0.Checked = true;
			}
		}

		private void Form_Shown(object sender, EventArgs e) {
			PopulateDescription();
			PopulateIcon();
		}

		private async void PopulateDescription() {
			try {
				txtDescription.Text = await HtmlConversion.ConvertHtmlToMarkdown(_artworkData.description);
			} catch (Exception) { }
			txtDescription.Enabled = true;
		}


		private async void PopulateIcon() {
			try {
				var character = await _client.GetCharacterAsync(_characterName);
				string avatar = character.Avatars.Tiny ?? character.Avatars.GetLargest();
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
			btnPost.Enabled = false;
			try {
				var user = await _client.GetUserAsync();
				var artwork = await _client.UploadArtwork(
					_characterName,
					_artworkData.data,
					_artworkData.GetContentType(),
					_artworkData.GetFileName());
				await _client.UpdateArtwork(artwork.Id, new FurryNetworkClient.UpdateArtworkParameters {
					Community_tags_allowed = chkFurryNetworkAllowCommunityTags.Checked,
					Description = txtDescription.Text,
					Publish = true,
					Rating = radFurryNetworkRating0.Checked ? 0
						: radFurryNetworkRating1.Checked ? 1
						: 2,
					Status = radFurryNetworkPublic.Checked ? "public"
						: radFurryNetworkUnlisted.Checked ? "unlisted"
						: "draft",
					Tags = txtTags.Text.Replace("#", " ").Split(' ').Where(s => s != ""),
					Title = txtTitle.Text
				});

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
