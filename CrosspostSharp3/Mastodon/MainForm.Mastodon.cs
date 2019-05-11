using System;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void mastodonToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.MastodonSettings>(
				s.Mastodon,
				async () => {
					using (var f = new MastodonLoginDialog()) {
						if (f.ShowDialog() == DialogResult.OK) {
							try {
								var oauth = await MapleFedNet.Api.Apps.Register(
									f.Instance,
									"CrosspostSharp",
									scopes: new[] {
										MapleFedNet.Model.Scope.Read,
										MapleFedNet.Model.Scope.Write
									});
								var token = await MapleFedNet.Api.OAuth.GetAccessTokenByPassword(
									f.Instance,
									oauth.ClientId,
									oauth.ClientSecret,
									f.Email,
									f.Password,
									MapleFedNet.Model.Scope.Read,
									MapleFedNet.Model.Scope.Write);
								var account = await MapleFedNet.Api.Accounts.VerifyCredentials(new MapleFedNet.Common.MastodonCredentials(f.Instance, token.AccessToken));
								return new[] {
									new Settings.MastodonSettings {
										Instance = f.Instance,
										accessToken = token.AccessToken,
										username = account.UserName
									}
								};
							} catch (Exception ex) {
								MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}

					return Enumerable.Empty<Settings.MastodonSettings>();
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Mastodon = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
