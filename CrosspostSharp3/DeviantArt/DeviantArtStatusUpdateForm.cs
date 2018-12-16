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
		private byte[] _image;

		private readonly DeviantArtClient _client;

		public DeviantArtStatusUpdateForm(IDeviantArtAccessToken token, IPostBase post) {
			InitializeComponent();
			_client = new DeviantArtClient(token);

			textBox1.Text = post.HTMLDescription;
			_image = (post as SavedPhotoPost)?.data;
		}

		private async void DeviantArtStatusUpdateForm_Shown(object sender, EventArgs e) {
			try {
				if (_image != null) {
					using (var ms = new MemoryStream(_image, false)) {
						picImageToPost.Image = Image.FromStream(ms);
					}
				} else {
					picImageToPost.Visible = false;
				}

				lblUsername1.Text = await _client.GetUsernameAsync();
				picUserIcon.ImageLocation = await _client.GetUserIconAsync();
			} catch (Exception) { }
		}
		
		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			
			try {
				long? itemId = null;

				if (picImageToPost.Image != null) {
					var req1 = new DeviantartApi.Requests.Stash.SubmitRequest {
						Data = _image,
						Title = "Status Update " + DateTime.Now,
						ArtistComments = textBox1.Text
					};
					var resp1 = await req1.ExecuteAsync();
					if (resp1.IsError) {
						throw new Exception(resp1.ErrorText);
					}
					if (!string.IsNullOrEmpty(resp1.Result.Error)) {
						throw new Exception(resp1.Result.ErrorDescription);
					}
					itemId = resp1.Result.ItemId;
				}

				await _client.UserStatusesPostAsync(new DeviantArtStatusPostParameters(textBox1.Text, null, null, itemId));

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
