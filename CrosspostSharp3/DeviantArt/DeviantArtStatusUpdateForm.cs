using DeviantArtFs;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
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
	public partial class DeviantArtStatusUpdateForm : Form {
		private SavedPhotoPost _image;

		private readonly DeviantArtClient _client;

		public DeviantArtStatusUpdateForm(IDeviantArtAccessToken token, IPostBase post) {
			InitializeComponent();
			_client = new DeviantArtClient(token);

			textBox1.Text = post.HTMLDescription;
			_image = post as SavedPhotoPost;
		}

		private async void DeviantArtStatusUpdateForm_Shown(object sender, EventArgs e) {
			try {
				if (_image != null) {
					using (var ms = new MemoryStream(_image.data, false)) {
						picImageToPost.Image = Image.FromStream(ms);
					}
				} else {
					picImageToPost.Visible = false;
				}

				lblUsername1.Text = await DeviantArtFs.User.Whoami.GetUsernameAsync(_client);
				picUserIcon.ImageLocation = await DeviantArtFs.User.Whoami.GetUserIconAsync(_client);
			} catch (Exception) { }
		}
		
		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			
			try {
				long? itemId = null;

				if (picImageToPost.Image != null) {
					itemId = await DeviantArtFs.Stash.Submit.StashSubmitAsync(_client, new DeviantArtFs.Stash.SubmitRequest(
						PostConverter.CreateFilename(_image),
						PostConverter.GetContentType(_image),
						_image.data));
				}

				await DeviantArtFs.User.StatusPost.UserStatusesPostAsync(_client, new DeviantArtFs.User.StatusPostRequest(textBox1.Text) {
					StashId = itemId
				});

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
