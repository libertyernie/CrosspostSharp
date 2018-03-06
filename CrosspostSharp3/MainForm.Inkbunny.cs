using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void inkbunnyToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.InkbunnySettings>(
				s.Inkbunny,
				async () => {
					using (var f = new UsernamePasswordDialog()) {
						f.Text = "Log In - Inkbunny";
						if (f.ShowDialog() == DialogResult.OK) {
							var client = await InkbunnyLib.InkbunnyClient.CreateAsync(f.Username, f.Password);
							return new[] {
								new Settings.InkbunnySettings {
									sid = client.Sid,
									userId = client.UserId,
									username = await client.GetUsernameAsync()
								}
							};
						} else {
							return Enumerable.Empty<Settings.InkbunnySettings>();
						}
					}
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
