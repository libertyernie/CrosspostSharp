using ArtSourceWrapper;
using ISchemm.WinFormsOAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void twitterToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.TwitterSettings>(
				s.Twitter,
				async () => {
					var oauth = new OAuthTwitter(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET);
					oauth.getRequestToken();
					string verifier = oauth.authorizeToken();
					if (verifier == null) return null;

					var settings = new Settings.TwitterSettings {
						tokenKey = oauth.getAccessToken(),
						tokenSecret = oauth.TokenSecret
					};

					try {
						var user = await Tweetinvi.Auth.ExecuteOperationWithCredentials(
							settings.GetCredentials(),
							() => Tweetinvi.UserAsync.GetAuthenticatedUser());
						settings.Username = user.ScreenName;
					} catch (Exception) { }
					
					return settings;
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Twitter = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
