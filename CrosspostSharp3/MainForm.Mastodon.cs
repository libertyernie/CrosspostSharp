using CrosspostSharp3.Mastodon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void mastodonToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.PleronetSettings> promptForCredentials() {
				using var f = new MastodonLoginDialog();
				if (f.ShowDialog() == DialogResult.OK) {
					var authClient = new Pleronet.AuthenticationClient(f.Instance);
					var appReg = await authClient.CreateApp("CrosspostSharp", Pleronet.Scope.Read | Pleronet.Scope.Write, "https://github.com/libertyernie/CrosspostSharp");
					var auth = await authClient.ConnectWithPassword(f.Email, f.Password);
					var account = await new Pleronet.MastodonClient(appReg, auth).GetCurrentUser();
					yield return new Settings.PleronetSettings {
						AppRegistration = appReg,
						Auth = auth,
						Username = account.UserName
					};
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PleronetSettings>(s.Pleronet, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.Pleronet = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
