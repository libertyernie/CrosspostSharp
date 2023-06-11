using ISchemm.WinFormsOAuth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void twitterToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.TwitterSettings> promptForCredentials() {
				var oauth = new OAuthTwitter(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET);
				oauth.getRequestToken();
				string verifier = oauth.authorizeToken();
				if (verifier == null) yield break;

				var settings = new Settings.TwitterSettings {
					tokenKey = oauth.getAccessToken(),
					tokenSecret = oauth.TokenSecret
				};

				var user = await settings.GetCredentials().Users.GetAuthenticatedUserAsync();
				settings.screenName = user.ScreenName;
				yield return settings;
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.TwitterSettings>(s.Twitter, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.Twitter = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
