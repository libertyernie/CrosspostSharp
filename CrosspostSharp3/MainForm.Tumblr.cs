using CrosspostSharp3.Tumblr;
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

					var token = new DontPanic.TumblrSharp.OAuth.Token(
						oauth.getAccessToken(),
						oauth.TokenSecret);

					var client = new TumblrClientFactory().Create<TumblrClient>(
						OAuthConsumer.Tumblr.CONSUMER_KEY,
						OAuthConsumer.Tumblr.CONSUMER_SECRET,
						token);
					var user = await client.GetUserInfoAsync();
					return TumblrChooseBlogs(token, user.Blogs);
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Tumblr = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}

		private static IEnumerable<Settings.TumblrSettings> TumblrChooseBlogs(DontPanic.TumblrSharp.OAuth.Token token, IEnumerable<UserBlogInfo> blogs) {
			using (var f = new TumblrBlogSelectionForm(blogs)) {
				f.ShowDialog();
				foreach (var blog in f.SelectedItems) {
					yield return new Settings.TumblrSettings {
						tokenKey = token.Key,
						tokenSecret = token.Secret,
						blogName = blog.Name
					};
				}
			}
		}
	}
}
