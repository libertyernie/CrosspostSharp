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
using WeasylLib;

namespace CrosspostSharp3.Weasyl {
	public partial class WeasylPostForm : Form {
		private readonly WeasylClient _apiClient;
		private readonly WeasylClient _frontendClient;
		private readonly SavedPhotoPost _artworkData;

		public WeasylPostForm(Settings.WeasylSettings s, SavedPhotoPost d) {
			InitializeComponent();

			_apiClient = _frontendClient = new WeasylClient(s.apiKey);
			_artworkData = d;

			txtTitle.Text = d.title;
			txtDescription.Text = d.description;
			txtTags.Text = string.Join(" ", d.tags.Select(t => t.Replace(' ', '_')));

			foreach (var o in Enum.GetValues(typeof(WeasylClient.SubmissionType))) {
				ddlCategory.Items.Add((WeasylClient.SubmissionType)o);
			}
			foreach (var o in Enum.GetValues(typeof(WeasylClient.Rating))) {
				ddlRating.Items.Add((WeasylClient.Rating)o);
			}
		}

		private async void WeasylPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _apiClient.WhoamiAsync();
				if (user?.login == null) {
					MessageBox.Show("You are not logged in.");
					Close();
					return;
				}
				lblUsername1.Text = user.login;

				var folders = await _frontendClient.GetFoldersAsync();
				ddlFolder.Items.Add("");
				foreach (var f in folders) ddlFolder.Items.Add(f);

				var avatarUrl = await _apiClient.GetAvatarUrlAsync(user.login);

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
				if (!(ddlCategory.SelectedItem is WeasylClient.SubmissionType subtype)) {
					throw new Exception("A category is required.");
				}
				if (!(ddlRating.SelectedItem is WeasylClient.Rating rating)) {
					throw new Exception("A rating is required.");
				}

				var folder = ddlFolder.SelectedItem as WeasylClient.Folder?;

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
