using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using ISchemm.WinFormsOAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void tumblrToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.TumblrSettings>(
				s.Tumblr,
				async () => {
					var oauth = new OAuthTumblr(OAuthConsumer.Tumblr.CONSUMER_KEY, OAuthConsumer.Tumblr.CONSUMER_SECRET);
					oauth.getRequestToken();
					string verifier = oauth.authorizeToken(); // display WebBrowser
					if (verifier == null) return null;
					
					var newTumblrSettings = new Settings.TumblrSettings {
						tokenKey = oauth.getAccessToken(),
						tokenSecret = oauth.TokenSecret
					};

					var client = new TumblrClientFactory().Create<TumblrClient>(
						OAuthConsumer.Tumblr.CONSUMER_KEY,
						OAuthConsumer.Tumblr.CONSUMER_SECRET,
						new DontPanic.TumblrSharp.OAuth.Token(
							newTumblrSettings.tokenKey,
							newTumblrSettings.tokenSecret));
					var user = await client.GetUserInfoAsync();
					using (var f = new TumblrBlogSelectionForm(user.Blogs)) {
						f.ShowDialog(this);
						var blog = f.SelectedItem;
						if (blog != null) {
							newTumblrSettings.blogName = blog.Name;
							newTumblrSettings.Username = blog.Name;
							return newTumblrSettings;
						} else {
							return null;
						}
					}
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Tumblr = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
