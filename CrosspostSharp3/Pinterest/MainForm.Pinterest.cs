using FurryNetworkLib;
using PinSharp;
using PinSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async void pinterestToolStripMenuItem_Click(object sender, EventArgs e) {
			toolsToolStripMenuItem.Enabled = false;

			Settings s = Settings.Load();
			using (var acctSelForm = new AccountSelectionForm<Settings.PinterestSettings>(
				s.Pinterest,
				async () => {
					using (var f = new PinterestAuthForm(OAuthConsumer.Pinterest.KEY, "https://www.example.org/", new[] { "read_public", "write_public" })) {
						f.ShowDialog();
						if (f.Code != null) {
							string token = await PinSharpAuthClient.GetAccessTokenAsync(
								OAuthConsumer.Pinterest.KEY,
								OAuthConsumer.Pinterest.SECRET,
								f.Code);
							var client = new PinSharpClient(token);
							var user = await client.Me.GetUserAsync();
							var boards = await client.Me.GetBoardsAsync();
							return PinterestChooseBoards(token, user.UserName, boards);
						} else {
							return Enumerable.Empty<Settings.PinterestSettings>();
						}
					}
				}
			)) {
				acctSelForm.ShowDialog(this);
				s.Pinterest = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}

		private static IEnumerable<Settings.PinterestSettings> PinterestChooseBoards(string accessToken, string username, IEnumerable<IUserBoard> boards) {
			using (var f = new PinterestBoardSelectionForm(boards)) {
				f.ShowDialog();
				foreach (var board in f.SelectedItems) {
					yield return new Settings.PinterestSettings {
						accessToken = accessToken,
						username = username,
						boardName = board.Name
					};
				}
			}
		}
	}
}
