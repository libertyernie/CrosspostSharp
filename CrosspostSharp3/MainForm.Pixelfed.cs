using Pleronet.Entities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private void pixelfedToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.PleronetSettings> promptForCredentials() {
				var p = new Settings.PleronetSettings();

				using (var f = new UsernamePasswordDialog()) {
					string resp = Microsoft.VisualBasic.Interaction.InputBox("Enter the Pixelfed domain / hostname:", this.Text, "");
					if (resp == "") yield break;

					p.AppRegistration = new AppRegistration { Instance = resp };
				}

				using (var f = new UsernamePasswordDialog()) {
					string resp = Microsoft.VisualBasic.Interaction.InputBox("Enter a personal access token with read and write permisssions:", this.Text, "");
					if (resp == "") yield break;

					p.Auth = new Auth { AccessToken = resp };
				}

				var client = p.GetClient();
				var account = await client.GetCurrentUser();
				p.Username = account.UserName;
				yield return p;
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PleronetSettings>(s.Pixelfed, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.Pixelfed = acctSelForm.CurrentList.ToList();
				s.Save();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
