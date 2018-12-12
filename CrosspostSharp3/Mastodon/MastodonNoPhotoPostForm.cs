using Mastonet;
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
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class MastodonNoPhotoPostForm : Form {
		private readonly MastodonClient _client;

		public MastodonNoPhotoPostForm(Settings.MastodonSettings s, IPostBase post) {
			InitializeComponent();
			_client = s.CreateClient();
			
			chkPotentiallySensitive.Checked = post.Mature || post.Adult;
			
			textBox1.Text = HtmlConversion.ConvertHtmlToText(post.HTMLDescription);
		}

		private async void TwitterPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _client.GetCurrentUser();
				lblUsername1.Text = user.DisplayName;
				lblUsername2.Text = "@" + user.UserName + "@" + _client.Instance;

				picUserIcon.ImageLocation = user.AvatarUrl;
			} catch (Exception) { }
		}
		
		private async void btnPost_Click(object sender, EventArgs e) {
			try {
				await _client.PostStatus(textBox1.Text, Visibility.Public, sensitive: chkPotentiallySensitive.Checked);
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
