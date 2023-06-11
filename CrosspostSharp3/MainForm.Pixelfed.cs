using Pleronet.Entities;
using Pleronet;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void pixelfedToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PleronetSettings>(
				s.Pixelfed,
				async () => {
					var p = new Settings.PleronetSettings();

					using (var f = new UsernamePasswordDialog()) {
						string resp = Microsoft.VisualBasic.Interaction.InputBox("Enter the Pixelfed domain / hostname:", this.Text, "");
						if (resp == "")
							return Enumerable.Empty<Settings.PleronetSettings>();

						p.AppRegistration = new AppRegistration { Instance = resp };
					}

					using (var f = new UsernamePasswordDialog()) {
						string resp = Microsoft.VisualBasic.Interaction.InputBox("Enter a personal access token with read and write permisssions:", this.Text, "");
						if (resp == "")
							return Enumerable.Empty<Settings.PleronetSettings>();

						p.Auth = new Auth { AccessToken = resp };
					}

					try {
						var client = p.GetClient();
						var account = await client.GetCurrentUser();
						p.Username = account.UserName;
						return new[] { p };
					} catch (Exception ex) {
						MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					return Enumerable.Empty<Settings.PleronetSettings>();
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Pixelfed = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
