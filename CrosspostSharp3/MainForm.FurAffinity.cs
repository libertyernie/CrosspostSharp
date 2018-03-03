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

			try {
				Settings s = Settings.Load();
				if (s.FurAffinity.a != null && s.FurAffinity.b != null) {
					var w = new FurAffinityIdWrapper(a: s.FurAffinity.a, b: s.FurAffinity.b);
					try {
						var user = await w.WhoamiAsync();
						if (MessageBox.Show(this, $"You are currenty logged into FurAffinity as {user}. Would you like to log out?", Text, MessageBoxButtons.YesNo) == DialogResult.Yes) {
							s.FurAffinity.a = null;
							s.FurAffinity.b = null;
							s.Save();
							MessageBox.Show(this, "You have been logged out.", Text);
							await ReloadWrapperList();
						} else {
							return;
						}
					} catch (Exception) { }
				}
				using (var f = new FAWinFormsLogin.loginPages.LoginFormFA()) {
					f.Text = "Log In - FurAffinity";
					if (f.ShowDialog() == DialogResult.OK) {
						s.FurAffinity.a = f.ACookie;
						s.FurAffinity.b = f.BCookie;
						s.Save();
						await ReloadWrapperList();
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
