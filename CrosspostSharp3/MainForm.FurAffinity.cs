using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void furAffinityToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.FurAffinitySettings>(
				s.FurAffinity,
				async () => {
					using (var f = new FAWinFormsLogin.loginPages.LoginFormFA()) {
						f.Text = "Log In - FurAffinity";
						if (f.ShowDialog() == DialogResult.OK) {
							return new Settings.FurAffinitySettings {
								username = await new FurAffinityIdWrapper(f.ACookie, f.BCookie).WhoamiAsync(),
								a = f.ACookie,
								b = f.BCookie
							};
						} else {
							return null;
						}
					}
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.FurAffinity = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
