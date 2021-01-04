using System;
using System.Linq;
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
							var client = await CrosspostSharp3.Inkbunny.InkbunnyClient.CreateAsync(f.Username, f.Password);
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
				},
				settings => {
					new Inkbunny.InkbunnyClient(settings.sid, settings.userId).LogoutAsync().ContinueWith(t => {
						if (t.Exception != null) MessageBox.Show(t.Exception.Message);
					});
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
