using CrosspostSharp3.Tumblr;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using ISchemm.WinFormsOAuth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private void tumblrToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.TumblrSettings> promptForCredentials() {
				var oauth = new OAuthTumblr(OAuthConsumer.Tumblr.CONSUMER_KEY, OAuthConsumer.Tumblr.CONSUMER_SECRET);
				oauth.getRequestToken();
				string verifier = oauth.authorizeToken(); // display WebBrowser
				if (verifier == null) yield break;

				var token = new DontPanic.TumblrSharp.OAuth.Token(
					oauth.getAccessToken(),
					oauth.TokenSecret);

				var client = new TumblrClientFactory().Create<TumblrClient>(
					OAuthConsumer.Tumblr.CONSUMER_KEY,
					OAuthConsumer.Tumblr.CONSUMER_SECRET,
					token);
				var user = await client.GetUserInfoAsync();

				using var f = new TumblrBlogSelectionForm(user.Blogs);
				f.ShowDialog();
				foreach (var blog in f.SelectedItems) {
					yield return new Settings.TumblrSettings {
						tokenKey = token.Key,
						tokenSecret = token.Secret,
						blogName = blog.Name
					};
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.TumblrSettings>(s.Tumblr, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.Tumblr = acctSelForm.CurrentList.ToList();
				s.Save();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
