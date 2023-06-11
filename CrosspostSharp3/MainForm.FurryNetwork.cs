using CrosspostSharp3.FurryNetwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private void furryNetworkToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			async IAsyncEnumerable<Settings.FurryNetworkSettings> promptForCredentials() {
				using var f1 = new FurryNetworkLoginForm();
				if (f1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					yield break;

				var client = new FurryNetworkClient(f1.RefreshToken);
				var user = await client.GetUserAsync();
				using var f2 = new FurryNetworkCharacterSelectionForm(user.characters);
				if (f2.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					foreach (var character in f2.SelectedItems) {
						yield return new Settings.FurryNetworkSettings {
							refreshToken = f1.RefreshToken,
							characterName = character.Name
						};
					}
				}
			}

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.FurryNetworkSettings>(s.FurryNetwork, () => promptForCredentials())) {
				acctSelForm.ShowDialog(this);
				s.FurryNetwork = acctSelForm.CurrentList.ToList();
				s.Save();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
