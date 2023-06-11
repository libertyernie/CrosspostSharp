using CrosspostSharp3.FurAffinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void furAffinityToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.FurAffinitySettings> promptForCredentials() {
				using var f = new FurAffinityLoginForm();
				f.Text = "Log In - FurAffinity";
				if (f.ShowDialog() == DialogResult.OK) {
					var newSettings = new Settings.FurAffinitySettings {
						a = f.ACookie,
						b = f.BCookie
					};
					newSettings.username = await FAExportArtworkSource.GetUsernameAsync($"b={f.BCookie}; a={f.ACookie}", false);
					yield return newSettings;
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.FurAffinitySettings>(s.FurAffinity, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.FurAffinity = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
