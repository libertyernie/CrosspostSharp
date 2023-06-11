using DeviantArtFs;
using DeviantArtFs.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private void deviantArtToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.DeviantArtAccountSettings> promptForCredentials() {
				using var f = new DeviantArtAuthorizationCodeForm(
					OAuthConsumer.DeviantArt.CLIENT_ID,
					new Uri("https://www.example.com"),
					new[] { "browse", "user", "stash", "publish", "user.manage" });
				f.Width = 1000;
				f.Height = 800;
				if (f.ShowDialog(this) == DialogResult.OK) {
					var a = new DeviantArtApp(OAuthConsumer.DeviantArt.CLIENT_ID.ToString(), OAuthConsumer.DeviantArt.CLIENT_SECRET);
					var token = await DeviantArtAuth.GetTokenAsync(a, f.Code, new Uri("https://www.example.com"));
					var u = await DeviantArtFs.Api.User.WhoamiAsync(token);
					yield return new Settings.DeviantArtAccountSettings {
						AccessToken = token.access_token,
						RefreshToken = token.refresh_token,
						Username = u.username
					};
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.DeviantArtAccountSettings>(s.DeviantArtAccounts, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.DeviantArtAccounts = acctSelForm.CurrentList.ToList();
				s.Save();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
