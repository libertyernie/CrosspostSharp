using ArtworkSourceSpecification;
using DeviantArtFs;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
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

namespace CrosspostSharp3.DeviantArt {
	public partial class DeviantArtStatusUpdateForm : Form {
		private readonly IDownloadedData _downloaded;

		private readonly IDeviantArtAccessToken _token;

		public DeviantArtStatusUpdateForm(IDeviantArtAccessToken token, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_token = token;
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

				var u = await DeviantArtFs.Api.User.WhoamiAsync(_token);
				lblUsername1.Text = u.username;
				picUserIcon.ImageLocation = u.usericon;
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;

			try {
				long? itemId = null;

				if (_downloaded != null) {
					var resp = await DeviantArtFs.Api.Stash.SubmitAsync(
						_token,
						DeviantArtFs.Api.Stash.SubmissionDestination.Default,
						DeviantArtFs.Api.Stash.SubmissionParameters.Default,
						_downloaded);
					itemId = resp.itemid;
				}

				await DeviantArtFs.Api.User.PostStatusAsync(
					_token,
					new DeviantArtFs.Api.User.EmbeddableStatusContent(
						DeviantArtFs.Api.User.EmbeddableObject.Nothing,
						DeviantArtFs.Api.User.EmbeddableObjectParent.NoParent,
						itemId is long x
							? DeviantArtFs.Api.User.EmbeddableStashItem.NewStashItem(x)
							: DeviantArtFs.Api.User.EmbeddableStashItem.NoStashItem),
					textBox1.Text);

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
