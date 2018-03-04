using ArtSourceWrapper;
using DeviantartApiLogin;
using DeviantArtControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class MainForm {
		private async Task<bool> UpdateDeviantArtTokens() {
			Settings s = Settings.Load();
			try {
				string currentToken = s.DeviantArt.RefreshToken;
				if (currentToken != null) {
					string newToken = await DeviantArtLoginStatic.UpdateTokens(
						OAuthConsumer.DeviantArt.CLIENT_ID,
						OAuthConsumer.DeviantArt.CLIENT_SECRET,
						currentToken);
					if (currentToken != newToken) {
						s.DeviantArt = new Settings.DeviantArtSettings {
							RefreshToken = newToken
						};
						s.Save();
					}
				}
				return true;
			} catch (Exception) { }
			return false;
		}

		private async void deviantArtToolStripMenuItem_Click(object sender, EventArgs e) {
			Settings s = Settings.Load();
			try {
				string currentToken = s.DeviantArt?.RefreshToken;
				if (currentToken != null) {
					string newToken = await DeviantArtLoginStatic.UpdateTokens(
						OAuthConsumer.DeviantArt.CLIENT_ID,
						OAuthConsumer.DeviantArt.CLIENT_SECRET,
						currentToken);
					if (currentToken != newToken) {
						s.DeviantArt = new Settings.DeviantArtSettings {
							RefreshToken = newToken
						};
						s.Save();
					}

					string user = await new StashWrapper().WhoamiAsync();
					if (MessageBox.Show(this, $"You are currenty logged into DeviantArt and sta.sh as {user}. Would you like to log out?", Text, MessageBoxButtons.YesNo) == DialogResult.Yes) {
						await DeviantArtDeviationWrapper.LogoutAsync();
						s.DeviantArt = new Settings.DeviantArtSettings {
							RefreshToken = null
						};
						s.Save();
						MessageBox.Show(this, "You have been logged out.", Text);
						await ReloadWrapperList();
					} else {
						return;
					}
				}
			} catch (DeviantArtException ex) when (ex.Message == "User canceled" || ex.Message == "The refresh_token is invalid.") {
				s.DeviantArt = new Settings.DeviantArtSettings {
					RefreshToken = null
				};
				s.Save();
				await ReloadWrapperList();
			} catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				MessageBox.Show(this, "Could not check DeviantArt login status", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			try {
				var result = await DeviantartApiLogin.WinForms.Login.SignInAsync(
						OAuthConsumer.DeviantArt.CLIENT_ID,
						OAuthConsumer.DeviantArt.CLIENT_SECRET,
						new Uri("https://www.example.com"),
						u => { },
						new[] { DeviantartApi.Login.Scope.Browse, DeviantartApi.Login.Scope.User, DeviantartApi.Login.Scope.Stash, DeviantartApi.Login.Scope.Publish });
				if (result.IsLoginError) {
					throw new Exception(result.LoginErrorText);
				}

				s.DeviantArt = new Settings.DeviantArtSettings {
					RefreshToken = result.RefreshToken
				};
				s.Save();
				await ReloadWrapperList();
			} catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				MessageBox.Show(this, "Could not log into DeviantArt", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
