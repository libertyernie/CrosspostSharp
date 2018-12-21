using DeviantArtFs;
using DeviantArtFs.WinForms;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void deviantArtToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.DeviantArtAccountSettings>(
				s.DeviantArtAccounts,
				async () => {
					using (var f = new DeviantArtAuthorizationCodeForm(
						OAuthConsumer.DeviantArt.CLIENT_ID,
						new Uri("https://www.example.com"),
						new[] { "browse", "user", "stash", "publish", "user.manage" })
					) {
						if (f.ShowDialog(this) == DialogResult.OK) {
							var a = new DeviantArtAuth(OAuthConsumer.DeviantArt.CLIENT_ID, OAuthConsumer.DeviantArt.CLIENT_SECRET);
							var token = await a.GetTokenAsync(f.Code, new Uri("https://www.example.com"));
							return new[] {
								new Settings.DeviantArtAccountSettings {
									AccessToken = token.AccessToken,
									ExpiresAt = token.ExpiresAt,
									RefreshToken = token.RefreshToken,
									Username = await DeviantArtFs.User.Whoami.GetUsernameAsync(token)
								}
							};
						}
					}

					return Enumerable.Empty<Settings.DeviantArtAccountSettings>();
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.DeviantArtAccounts = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
