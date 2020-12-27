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
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		private readonly IDeviantArtAccessToken _token;

		public DeviantArtStatusUpdateForm(IDeviantArtAccessToken token, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_token = token;
			_post = post;
			_downloaded = downloaded;

			textBox1.Text = post.HTMLDescription;
		}

		private async void DeviantArtStatusUpdateForm_Shown(object sender, EventArgs e) {
			try {
				if (_downloaded != null) {
					using (var ms = new MemoryStream(_downloaded.Data, false)) {
						picImageToPost.Image = Image.FromStream(ms);
					}
				} else {
					picImageToPost.Visible = false;
				}

				var u = await DeviantArtFs.Api.User.Whoami.ExecuteAsync(_token, DeviantArtObjectExpansion.None);
				lblUsername1.Text = u.username;
				picUserIcon.ImageLocation = u.usericon;
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;

			try {
				long? itemId = null;

				if (_downloaded != null) {
					var resp = await DeviantArtFs.Api.Stash.Submit.ExecuteAsync(_token, new DeviantArtFs.Api.Stash.SubmitRequest(
						_downloaded.Filename,
						_downloaded.ContentType,
						_downloaded.Data));
					itemId = resp.itemid;
				}

				await DeviantArtFs.Api.User.StatusPost.ExecuteAsync(_token, new DeviantArtFs.Api.User.StatusPostRequest(textBox1.Text) {
					Stashid = itemId
				});

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
