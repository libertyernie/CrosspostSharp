using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void PixivToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PixivUploadSettings>(
				s.PixivUpload,
				() => {
					using (var f = new UsernamePasswordDialog()) {
						f.UsernameLabel = "PHPSESSID";
						f.ShowPassword = false;
						if (f.ShowDialog() == DialogResult.OK) {
							return new[] {
								new Settings.PixivUploadSettings {
									PHPSESSID = f.Username
								}
							};
						}
					}

					return Enumerable.Empty<Settings.PixivUploadSettings>();
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.PixivUpload = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
