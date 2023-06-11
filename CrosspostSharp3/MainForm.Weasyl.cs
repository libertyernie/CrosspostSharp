using CrosspostSharp3.Weasyl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void weasylToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.WeasylSettings> promptForCredentials() {
				using var f = new UsernamePasswordDialog();
				f.UsernameLabel = "API Key";
				f.ShowPassword = false;
				if (f.ShowDialog() == DialogResult.OK) {
					var client = new WeasylClient(f.Username);
					var user = await client.WhoamiAsync();
					yield return new Settings.WeasylSettings {
						username = user.login,
						apiKey = f.Username
					};
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.WeasylSettings>(s.WeasylApi, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.WeasylApi = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
