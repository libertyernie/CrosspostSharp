using ArtSourceWrapper;
using ISchemm.WinFormsOAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void mediaRSSToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.MediaRSSSettings>(
				s.MediaRSS,
				async () => {
					using (var form = new AddRSSForm()) {
						if (form.ShowDialog(this) == DialogResult.OK) {
							if (MessageBox.Show(this, $"If this feed contains content that is not your work, you are responsible for observing copyright laws. Don't repost copyrighted artwork or photos without permission.", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
								return new Settings.MediaRSSSettings {
									Username = form.FeedLabel,
									url = form.FeedUrl
								};
							}
						}
						return null;
					}
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.MediaRSS = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
