using Mastonet;
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
								var authClient = new AuthenticationClient(f.Instance);
								var appRegistration = await authClient.CreateApp("CrosspostSharp", Scope.Read | Scope.Write, website: "https://github.com/libertyernie/CrosspostSharp");
								var auth = await authClient.ConnectWithPassword(f.Email, f.Password);
								var client = new MastodonClient(appRegistration, auth);
								var account = await client.GetCurrentUser();
								return new[] {
									new Settings.MastodonSettings {
										appRegistration = appRegistration,
										auth = auth,
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
