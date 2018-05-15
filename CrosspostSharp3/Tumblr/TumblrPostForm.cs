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
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class TumblrPostForm : Form {
		private readonly TumblrClient _client;
		private readonly string _blogName;
		private readonly ArtworkData _artworkData;

		public TumblrPostForm(Settings.TumblrSettings s, ArtworkData d) {
			InitializeComponent();
			_client = new TumblrClientFactory().Create<TumblrClient>(
				OAuthConsumer.Tumblr.CONSUMER_KEY,
				OAuthConsumer.Tumblr.CONSUMER_SECRET,
				new Token(s.tokenKey, s.tokenSecret));
			_artworkData = d;
			_blogName = s.blogName;
			lblUsername1.Text = _blogName;

			if (string.IsNullOrEmpty(_artworkData.title)) {
				chkIncludeTitle.Enabled = false;
				chkIncludeTitle.Checked = false;
			}
			if (string.IsNullOrEmpty(_artworkData.description)) {
				chkIncludeDescription.Enabled = false;
				chkIncludeDescription.Checked = false;
			}
			if (string.IsNullOrEmpty(_artworkData.url)) {
				chkIncludeLink.Enabled = false;
				chkIncludeLink.Checked = false;
			}

			txtTags.Text = string.Join(", ", new[] { "my art" }.Concat(d.tags));

			chkIncludeTitle.CheckedChanged += (o, e) => UpdateText();
			chkIncludeDescription.CheckedChanged += (o, e) => UpdateText();
			chkIncludeLink.CheckedChanged += (o, e) => UpdateText();

			using (var ms = new MemoryStream(d.data, false))
			using (var image = Image.FromStream(ms)) {
				chkMakeSquare.Checked = image.Height > image.Width;
			}

			UpdateText();
		}

		private async void TumblrPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _client.GetUserInfoAsync();
				lblUsername2.Text = "Logged in as: " + user.Name;

				var req = WebRequestFactory.Create($"https://api.tumblr.com/v2/blog/{_blogName}.tumblr.com/avatar/{picUserIcon.Width}");
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					picUserIcon.Image = Image.FromStream(ms);
				}
			} catch (Exception) { }
		}

		private void UpdateText() {
			StringBuilder sb = new StringBuilder();
			if (chkIncludeTitle.Checked) {
				sb.AppendLine($"<p><b>{WebUtility.HtmlEncode(_artworkData.title)}</b></p>");
			}
			if (chkIncludeDescription.Checked) {
				sb.AppendLine(_artworkData.description);
			}
			if (chkIncludeLink.Checked) {
				sb.AppendLine($"<p><a href='{_artworkData.url}'>{WebUtility.HtmlEncode(_artworkData.url)}</a></p>");
			}
			textBox1.Text = sb.ToString();
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;

			byte[] data = _artworkData.data;
			if (chkMakeSquare.Checked) {
				using (var ms1 = new MemoryStream(data, false))
				using (var image1 = Image.FromStream(ms1)) {
					int size = Math.Max(image1.Width, image1.Height);
					using (var image2 = new Bitmap(size, size))
					using (var g2 = Graphics.FromImage(image2))
					using (var ms2 = new MemoryStream()) {
						g2.DrawImage(image1,
							(image2.Width - image1.Width) / 2,
							(image2.Height - image1.Height) / 2,
							image1.Width,
							image1.Height);
						image2.Save(ms2, image1.RawFormat);
						data = ms2.ToArray();
					}
				}
			}

			try {
				BinaryFile imageToPost = new BinaryFile(data,
					_artworkData.GetFileName(),
					_artworkData.GetContentType());
				PostData post = PostData.CreatePhoto(
					new BinaryFile[] { imageToPost },
					textBox1.Text,
					_artworkData.url,
					txtTags.Text.Replace("#", "").Split(',').Select(s => s.Trim()).Where(s => s != ""));
				PostCreationInfo info = await _client.CreatePostAsync(_blogName, post);
				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
