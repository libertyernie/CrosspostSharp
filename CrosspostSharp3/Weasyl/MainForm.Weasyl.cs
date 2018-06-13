using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeasylLib.Frontend;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void weasylToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.WeasylSettings>(
				s.Weasyl,
				async () => {
					using (var f = new UsernamePasswordDialog()) {
						if (f.ShowDialog() == DialogResult.OK) {
							try {
								var client = new WeasylFrontendClient();
								await client.SignInAsync(f.Username, f.Password);
								string username = await client.GetUsernameAsync();
								if (username == null) {
									throw new Exception("Username/password invalid");
								}
								return new[] {
									new Settings.WeasylSettings {
										username = username,
										wzl = client.WZL
									}
								};
							} catch (Exception ex) {
								MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}
					
					return Enumerable.Empty<Settings.WeasylSettings>();
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Weasyl = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
