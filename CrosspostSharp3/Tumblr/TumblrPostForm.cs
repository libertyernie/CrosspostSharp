using ArtworkSourceSpecification;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using SourceWrappers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class TumblrPostForm : Form {
		private readonly TumblrClient _client;
		private readonly string _blogName;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public TumblrPostForm(Settings.TumblrSettings s, TextPost post, IDownloadedData downloaded = null) {
			InitializeComponent();
			_client = new TumblrClientFactory().Create<TumblrClient>(
				OAuthConsumer.Tumblr.CONSUMER_KEY,
				OAuthConsumer.Tumblr.CONSUMER_SECRET,
				new Token(s.tokenKey, s.tokenSecret));
			_blogName = s.blogName;
			lblUsername1.Text = _blogName;

			_post = post;
			_downloaded = downloaded;
			txtTitle.Text = post.Title;
			txtDescription.Text = post.HTMLDescription;
			txtTags.Text = string.Join(", ", post.Tags);
			chkIncludeImage.Enabled = chkMakeSquare.Enabled = downloaded != null;
			chkIncludeImage.Checked = downloaded != null;

			picUserIcon.ImageLocation = $"https://api.tumblr.com/v2/blog/{_blogName}.tumblr.com/avatar/{picUserIcon.Width}";
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e) {
			chkMakeSquare.Enabled = chkIncludeImage.Checked;
			txtTitle.Enabled = !chkIncludeImage.Checked;
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			try {
				if (_downloaded != null && chkIncludeImage.Checked) {
					byte[] data = _downloaded?.Data;
					if (data != null && chkMakeSquare.Checked) {
						data = ImageUtils.MakeSquare(data);
					}
					BinaryFile imageToPost = new BinaryFile(data,
						_downloaded.Filename,
						_downloaded.ContentType);
					PostData post = PostData.CreatePhoto(
						imageToPost,
						txtDescription.Text,
						txtTags.Text.Replace("#", "").Split(',').Select(s => s.Trim()).Where(s => s != ""));
					PostCreationInfo info = await _client.CreatePostAsync(_blogName, post);
				} else {
					PostData post = PostData.CreateText(
						txtTitle.Text,
						txtDescription.Text,
						tags: txtTags.Text.Replace("#", "").Split(',').Select(s => s.Trim()).Where(s => s != ""));
					PostCreationInfo info = await _client.CreatePostAsync(_blogName, post);
				}
				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
