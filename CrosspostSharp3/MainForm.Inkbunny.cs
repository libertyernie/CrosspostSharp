using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void inkbunnyToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.InkbunnySettings> promptForCredentials() {
				using var f = new UsernamePasswordDialog();
				f.Text = "Log In - Inkbunny";
				if (f.ShowDialog() == DialogResult.OK) {
					var client = await Inkbunny.InkbunnyClient.CreateAsync(f.Username, f.Password);
					yield return new Settings.InkbunnySettings {
						sid = client.Sid,
						userId = client.UserId,
						username = await client.GetUsernameAsync()
					};
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.InkbunnySettings>(
				s.Inkbunny,
				() => promptForCredentials(),
				settings => {
					new Inkbunny.InkbunnyClient(settings.sid, settings.userId).LogoutAsync().ContinueWith(t => {
						if (t.Exception != null) MessageBox.Show(t.Exception.Message);
					});
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Inkbunny = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
