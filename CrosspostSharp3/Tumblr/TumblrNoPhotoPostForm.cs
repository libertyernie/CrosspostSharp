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
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class TumblrNoPhotoPostForm : Form {
		private readonly TumblrClient _client;
		private readonly string _blogName;

		public TumblrNoPhotoPostForm(Settings.TumblrSettings s, IPostBase post) {
			InitializeComponent();
			_client = new TumblrClientFactory().Create<TumblrClient>(
				OAuthConsumer.Tumblr.CONSUMER_KEY,
				OAuthConsumer.Tumblr.CONSUMER_SECRET,
				new Token(s.tokenKey, s.tokenSecret));
			_blogName = s.blogName;
			lblUsername1.Text = _blogName;

			textBox1.Text = post.HTMLDescription;
			txtTags.Text = string.Join(", ", post.Tags);
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
		
		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			
			try {
				PostData post = PostData.CreateText(
					textBox1.Text,
					tags: txtTags.Text.Replace("#", "").Split(',').Select(s => s.Trim()).Where(s => s != ""));
				PostCreationInfo info = await _client.CreatePostAsync(_blogName, post);
				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
