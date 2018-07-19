using PillowfortFs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private Dictionary<string, PillowfortClient> _pfcs;

		private IReadOnlyDictionary<string, PillowfortClient> PillowfortClients {
			get {
				if (_pfcs == null) {
					ReloadPillowfortClients();
				}
				return _pfcs;
			}
		}

		private void ReloadPillowfortClients() {
			Settings s = Settings.Load();
			_pfcs = s.Pillowfort.ToDictionary(x => x.username, x => new PillowfortClient { Cookie = x.cookie });
		}

		private void SavePillowfortSettings() {
			if (PillowfortClients.Any()) {
				Settings s = Settings.Load();
				s.Pillowfort = PillowfortClients
					.Select(p => new Settings.PillowfortSettings {
						username = p.Key,
						cookie = p.Value.Cookie
					})
					.Where(p => p.cookie != null)
					.ToList();
				s.Save();
			}
		}

		private async void pillowfortToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PillowfortSettings>(
				s.Pillowfort,
				async () => {
					using (var f = new UsernamePasswordDialog()) {
						f.UsernameLabel = "Email";
						if (f.ShowDialog() == DialogResult.OK) {
							try {
								var client = await PillowfortClientFactory.CreateClientAsync(f.Username, f.Password);
								string username = await client.WhoamiAsync();
								return new[] {
									new Settings.PillowfortSettings {
										username = username,
										cookie = client.Cookie
									}
								};
							} catch (Exception ex) {
								MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}

					return Enumerable.Empty<Settings.PillowfortSettings>();
				},
				x => {
					new PillowfortClient { Cookie = x.cookie }
						.SignoutAsync()
						.GetAwaiter()
						.GetResult();
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Pillowfort = acctSelForm.CurrentList.ToList();
				s.Save();
				ReloadPillowfortClients();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}
	}
}
