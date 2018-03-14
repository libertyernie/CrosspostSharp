using FurryNetworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void pixivToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PixivSettings>(
				s.Pixiv,
				() => GetNewPixivAccount()
			)) {
				acctSelForm.ShowDialog(this);
				s.Pixiv = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}

		private static IEnumerable<Settings.PixivSettings> GetNewPixivAccount() {
			using (var f = new UsernamePasswordDialog()) {
				if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					yield return new Settings.PixivSettings {
						username = f.Username,
						password = f.Password
					};
				}
			}
		}
	}
}
