using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void weasylToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.WeasylSettings>(
				s.WeasylApi,
				async () => {
					using (var f = new UsernamePasswordDialog()) {
						f.UsernameLabel = "API Key";
						f.ShowPassword = false;
						if (f.ShowDialog() == DialogResult.OK) {
							try {
								var wrapper = new WeasylSourceWrapper(f.Username, loadAll: true);
								string username = await wrapper.WhoamiAsync();
								if (username == null) {
									throw new Exception("Username/password invalid");
								}
								return new[] {
									new Settings.WeasylSettings {
										username = username,
										apiKey = f.Username
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
				s.WeasylApi = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
