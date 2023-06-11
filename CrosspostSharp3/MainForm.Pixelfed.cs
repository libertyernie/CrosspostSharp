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
			using (var acctSelForm = new AccountSelectionForm<Settings.PixelfedSettings>(
				s.Pixelfed,
				async () => {
					var p = new Settings.PixelfedSettings();

					using (var f = new UsernamePasswordDialog()) {
						f.UsernameLabel = "Host";
						f.ShowPassword = false;
						if (f.ShowDialog() != DialogResult.OK)
							return Enumerable.Empty<Settings.PixelfedSettings>();

						p.host = f.Username;
					}

					using (var f = new UsernamePasswordDialog()) {
						f.UsernameLabel = "Token";
						f.ShowPassword = false;
						if (f.ShowDialog() != DialogResult.OK)
							return Enumerable.Empty<Settings.PixelfedSettings>();
						p.token = f.Username;
					}

					try {
						var client = p.GetClient();
						var account = await client.GetCurrentUser();
						p.username = account.UserName;
						return new[] { p };
					} catch (Exception ex) {
						MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					return Enumerable.Empty<Settings.PixelfedSettings>();
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
