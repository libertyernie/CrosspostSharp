using ISchemm.WinFormsOAuth;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void flickrToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.FlickrSettings>(
				s.Flickr,
				async () => {
					var oauth = new OAuthFlickr(OAuthConsumer.Flickr.KEY, OAuthConsumer.Flickr.SECRET);
					oauth.getRequestToken();
					string verifier = oauth.authorizeToken(); // display WebBrowser
					if (verifier == null) return Enumerable.Empty<Settings.FlickrSettings>();

					string accessToken = oauth.getAccessToken();
					string accessTokenSecret = oauth.TokenSecret;

					var settings = new Settings.FlickrSettings {
						tokenKey = accessToken,
						tokenSecret = accessTokenSecret
					};

					string username = await new FlickrSourceWrapper(settings.CreateClient()).WhoamiAsync();

					settings.username = username;
					return new[] { settings };
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Flickr = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
