using FurryNetworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void furryNetworkToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.FurryNetworkSettings>(
				s.FurryNetwork,
				async () => {
					using (var f = new FurryNetworkLoginForm()) {
						if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
							return Enumerable.Empty<Settings.FurryNetworkSettings>();

						var client = new FurryNetworkClient(f.RefreshToken);
						var user = await client.GetUserAsync();
						return FurryNetworkChooseCharacters(client.RefreshToken, user.characters);
					}
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.FurryNetwork = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}

		private static IEnumerable<Settings.FurryNetworkSettings> FurryNetworkChooseCharacters(string refreshToken, IEnumerable<Character> characters) {
			using (var f = new FurryNetworkCharacterSelectionForm(characters)) {
				if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					foreach (var character in f.SelectedItems) {
						yield return new Settings.FurryNetworkSettings {
							refreshToken = refreshToken,
							characterName = character.Name
						};
					}
				}
			}
		}
	}
}
