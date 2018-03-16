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
			Settings s = Settings.Load();
			if (string.IsNullOrEmpty(s.Pinterest.appId) || string.IsNullOrEmpty(s.Pinterest.appSecret)) {
				using (var f = new PinterestAppForm()) {
					if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
						return;
					}
					s.Pinterest.appId = f.AppId;
					s.Pinterest.appSecret = f.AppSecret;
					s.Save();
				}
			}

			toolsToolStripMenuItem.Enabled = false;

			using (var acctSelForm = new AccountSelectionForm<Settings.PinterestSettings>(
				s.Pinterest.accounts,
				async () => {
					using (var f = new PinterestAuthForm(s.Pinterest.appId, "https://www.example.org/", new[] { "read_public", "write_public" })) {
						f.ShowDialog();
						if (f.Code != null) {
							string token = await PinSharpAuthClient.GetAccessTokenAsync(
								s.Pinterest.appId,
								s.Pinterest.appSecret,
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
				s.Pinterest.accounts = acctSelForm.CurrentList.ToList();
				s.Save();
				await ReloadWrapperList();
			}

			toolsToolStripMenuItem.Enabled = true;
		}

		private static IEnumerable<Settings.PinterestSettings> PinterestChooseBoards(string accessToken, string username, IEnumerable<IUserBoard> boards) {
			using (var f = new PinterestBoardSelectionForm(boards)) {
				f.ShowDialog();
				foreach (var board in f.SelectedItems) {
					Uri uri = new Uri(board.Url);
					string boardName = uri.AbsolutePath;
					while (boardName.StartsWith("/")) boardName = boardName.Substring(1);
					while (boardName.EndsWith("/")) boardName = boardName.Substring(0, boardName.Length - 1);
					yield return new Settings.PinterestSettings {
						accessToken = accessToken,
						username = username,
						boardName = boardName
					};
				}
			}
		}
	}
}
