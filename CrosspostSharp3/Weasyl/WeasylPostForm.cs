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
using WeasylLib.Api;
using WeasylLib.Frontend;

namespace CrosspostSharp3.Weasyl {
	public partial class WeasylPostForm : Form {
		private readonly WeasylFrontendClient _frontendClient;
		private readonly WeasylApiClient _apiClient;
		private readonly ArtworkData _artworkData;

		public WeasylPostForm(Settings.WeasylSettings s, ArtworkData d) {
			InitializeComponent();

			_frontendClient = new WeasylFrontendClient {
				WZL = s.wzl
			};
			_apiClient = new WeasylApiClient();
			_artworkData = d;

			txtTitle.Text = d.title;
			txtDescription.Text = d.description;
			txtTags.Text = string.Join(" ", d.tags.Select(t => t.Replace(' ', '_')));

			foreach (var o in Enum.GetValues(typeof(WeasylFrontendClient.SubmissionType))) {
				ddlCategory.Items.Add((WeasylFrontendClient.SubmissionType)o);
			}
			foreach (var o in Enum.GetValues(typeof(WeasylFrontendClient.Rating))) {
				ddlRating.Items.Add((WeasylFrontendClient.Rating)o);
			}
		}

		private async void WeasylPostForm_Shown(object sender, EventArgs e) {
			try {
				var username = await _frontendClient.GetUsernameAsync();
				if (username == null) {
					MessageBox.Show("You are not logged in.");
					Close();
					return;
				}
				lblUsername1.Text = username;

				var folders = await _frontendClient.GetFoldersAsync();
				ddlFolder.Items.Add("");
				foreach (var f in folders) ddlFolder.Items.Add(f);

				var avatarUrl = await _apiClient.GetAvatarUrlAsync(username);

				var req = WebRequestFactory.Create(avatarUrl);
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
			btnPost.Enabled = false;
			try {
				if (!(ddlCategory.SelectedItem is WeasylFrontendClient.SubmissionType subtype)) {
					throw new Exception("A category is required.");
				}
				if (!(ddlRating.SelectedItem is WeasylFrontendClient.Rating rating)) {
					throw new Exception("A rating is required.");
				}

				var folder = ddlFolder.SelectedItem as WeasylFrontendClient.Folder?;

				await _frontendClient.UploadVisualAsync(
					_artworkData.data,
					_artworkData.title,
					subtype,
					folder?.FolderId,
					rating,
					txtDescription.Text,
					txtTags.Text.Split(' '));

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
