using ArtSourceWrapper;
using DeviantArtControls;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FAWinFormsLogin.loginPages;
using FurryNetworkLib;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;

namespace CrosspostSharp {
	public partial class SettingsDialog : Form {
		public Settings Settings { get; private set; }

        public SettingsDialog(Settings settings) {
			InitializeComponent();
			this.Settings = settings.Copy();
            FillForm();
		}

		private void btnSave_Click(object sender, EventArgs e) {
			Settings.Weasyl.APIKey = txtWeasylAPIKey.Text;
			if (Settings.Weasyl.APIKey == "") Settings.Weasyl.APIKey = null;

			Settings.Tumblr.BlogName = txtBlogName.Text;
			Settings.Tumblr.AutoSidePadding = chkSidePadding.Checked;

			Settings.Defaults.HeaderHTML = txtHeader.Text;
			Settings.Defaults.FooterHTML = txtFooter.Text;
			Settings.Defaults.Tags = txtTags.Text;

			Settings.Save();
        }

        private async void btnDeviantArtSignIn_Click(object sender, EventArgs e) {
            try {
                if (string.Equals("Sign out", btnDeviantArtSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
                    await DeviantArtLoginStatic.LogoutAsync();
                    Settings.DeviantArt.RefreshToken = null;
                } else {
                    var result = await DeviantartApiLogin.WinForms.Login.SignInAsync(
                        OAuthConsumer.DeviantArt.CLIENT_ID,
                        OAuthConsumer.DeviantArt.CLIENT_SECRET,
                        new Uri("https://www.example.com"),
                        s => { },
                        new[] { DeviantartApi.Login.Scope.Browse, DeviantartApi.Login.Scope.User, DeviantartApi.Login.Scope.Stash, DeviantartApi.Login.Scope.Publish, DeviantartApi.Login.Scope.UserManage });
                    if (result.IsLoginError) {
                        throw new Exception(result.LoginErrorText);
                    }

                    Settings.DeviantArt.RefreshToken = result.RefreshToken;
                }
                UpdateDeviantArtTokenLabelAsync();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void btnTumblrSignIn_Click(object sender, EventArgs e) {
			if (string.Equals("Sign out", btnTumblrSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
				Settings.TumblrToken = null;
			} else {
				Settings.TumblrToken = TumblrKey.Obtain(OAuthConsumer.Tumblr.CONSUMER_KEY, OAuthConsumer.Tumblr.CONSUMER_SECRET);
			}
			UpdateTumblrTokenLabel();
        }

        private async void btnInkbunnySignIn_Click(object sender, EventArgs e) {
            if (string.Equals("Sign out", btnInkbunnySignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
                Settings.Inkbunny.Sid = null;
                Settings.Inkbunny.UserId = null;
            } else {
                using (LoginDialog d = new LoginDialog()) {
                    if (d.ShowDialog() != DialogResult.OK) return;
                    try {
                        var client = await InkbunnyLib.InkbunnyClient.CreateAsync(d.Username, d.Password);
                        Settings.Inkbunny.Sid = client.Sid;
                        Settings.Inkbunny.UserId = client.UserId;
                    } catch (Exception ex) {
                        MessageBox.Show("Could not log into Inkbunny: " + ex.Message);
                    }
                }
            }
            UpdateInkbunnyTokenLabel();
        }

        private void btnTwitterSignIn_Click(object sender, EventArgs e) {
			if (string.Equals("Sign out", btnTwitterSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
				Settings.TwitterCredentials = null;
			} else {
				Settings.TwitterCredentials = TwitterKey.Obtain(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET);
			}
			UpdateTwitterTokenLabel();
        }

        private void btnFurAffinitySignIn_Click(object sender, EventArgs e) {
            if (string.Equals("Sign out", btnFurAffinitySignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
                Settings.FurAffinity.b = null;
                Settings.FurAffinity.a = null;
            } else {
                using (var f = new LoginFormFA()) {
                    if (f.ShowDialog() == DialogResult.OK) {
                        Settings.FurAffinity.b = f.BCookie;
                        Settings.FurAffinity.a = f.ACookie;
                    }
                }
            }
            UpdateFurAffinityTokenLabel();
        }

        private void btnFlickrSignIn_Click(object sender, EventArgs e) {
            if (string.Equals("Sign out", btnFlickrSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
                Settings.Flickr = null;
            } else {
                Settings.Flickr = FlickrKey.Obtain(OAuthConsumer.Flickr.KEY, OAuthConsumer.Flickr.SECRET);
            }
            UpdateFlickrTokenLabel();
		}

		private async void btnPixivSignIn_Click(object sender, EventArgs e) {
			if (string.Equals("Sign out", btnPixivSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
				Settings.Pixiv.Username = null;
				Settings.Pixiv.Password = null;
			} else {
				using (var f = new UsernamePasswordDialog()) {
					if (f.ShowDialog() == DialogResult.OK) {
						try {
							var authResult = await Pixeez.Auth.AuthorizeAsync(f.Username, f.Password, null, "cpsharp");
							Settings.Pixiv.Username = authResult.Key.Username;
							Settings.Pixiv.Password = authResult.Key.Password;
						} catch (Exception ex) {
							MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
			}
			UpdatePixivTokenLabel();
		}

		private async void btnFurryNetworkSignIn_Click(object sender, EventArgs e) {
			if (string.Equals("Sign out", btnFurryNetworkSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
				Settings.FurryNetwork.RefreshToken = null;
			} else {
				using (var f = new UsernamePasswordDialog()) {
					f.UsernameLabel = "Email";
					if (f.ShowDialog() == DialogResult.OK) {
						try {
							var client = await FurryNetworkClient.LoginAsync(f.Username, f.Password);
							Settings.FurryNetwork.RefreshToken = client.RefreshToken;
						} catch (Exception ex) {
							MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
			}
			UpdateFurryNetworkTokenLabel();
		}

		private void FillForm() {
			txtWeasylAPIKey.Text = Settings.Weasyl.APIKey ?? "";

			txtBlogName.Text = Settings.Tumblr.BlogName ?? "";
			chkSidePadding.Checked = Settings.Tumblr.AutoSidePadding;

			txtHeader.Text = Settings.Defaults.HeaderHTML ?? "";
			txtFooter.Text = Settings.Defaults.FooterHTML ?? "";
			txtTags.Text = Settings.Defaults.Tags;

            UpdateDeviantArtTokenLabelAsync();
            UpdateTumblrTokenLabel();
			UpdateTwitterTokenLabel();
            UpdateInkbunnyTokenLabel();
            UpdateFurAffinityTokenLabel();
            UpdateFlickrTokenLabel();
			UpdatePixivTokenLabel();
		}

        private async void UpdateDeviantArtTokenLabelAsync() {
            if (Settings.DeviantArt.RefreshToken != null) {
                btnDeviantArtSignIn.Text = "Sign out";
                try {
					string username = await new DeviantArtSourceWrapper().AsISourceWrapper().WhoamiAsync();
                    lblDeviantArtTokenStatus.ForeColor = Color.Green;
                    lblDeviantArtTokenStatus.Text = $"{username} ({Settings.DeviantArt.RefreshToken}...)";
                } catch (Exception e) {
                    lblDeviantArtTokenStatus.ForeColor = Color.Red;
                    lblDeviantArtTokenStatus.Text = $"{e.Message} ({Settings.DeviantArt.RefreshToken}...)";
                }
            } else {
				btnDeviantArtSignIn.Text = "Sign in";
                lblDeviantArtTokenStatus.Text = "Not signed in";
			}
        }

        private async void UpdateTumblrTokenLabel() {
			Token token = Settings.TumblrToken;
			if (token.IsValid) {
				btnTumblrSignIn.Text = "Sign out";
				using (TumblrClient client = new TumblrClientFactory().Create<TumblrClient>(
					OAuthConsumer.Tumblr.CONSUMER_KEY,
					OAuthConsumer.Tumblr.CONSUMER_SECRET,
					token
				)) {
					try {
						UserInfo u = await client.GetUserInfoAsync();
						lblTokenStatus.ForeColor = Color.Green;
                        lblTokenStatus.Text = $"{u.Name} ({token.Key}...)";
					} catch (Exception e) {
						lblTokenStatus.ForeColor = Color.Red;
						lblTokenStatus.Text = $"{e.Message} ({token.Key}...)";
					}
				}
			} else {
				lblTokenStatus.ForeColor = SystemColors.WindowText;
				btnTumblrSignIn.Text = "Sign in";
				lblTokenStatus.Text = "Not signed in";
			}
		}

        private void UpdateInkbunnyTokenLabel() {
            if (Settings.Inkbunny.Sid == null) {
                btnInkbunnySignIn.Text = "Sign in";
                lblInkbunnyTokenStatus.ForeColor = SystemColors.WindowText;
                lblInkbunnyTokenStatus.Text = "";
            } else {
                btnInkbunnySignIn.Text = "Sign out";
                lblInkbunnyTokenStatus.ForeColor = Color.Green;
                lblInkbunnyTokenStatus.Text = Settings.Inkbunny.Sid + "...";
            }
        }

        private void UpdateTwitterTokenLabel() {
			var twitterCredentials = Settings.TwitterCredentials;
			try {
				string screenName = twitterCredentials?.GetScreenName();
				if (screenName == null) {
					btnTwitterSignIn.Text = "Sign in";
					lblTwitterTokenStatus.ForeColor = SystemColors.WindowText;
					lblTwitterTokenStatus.Text = "Not signed in";
				} else {
					btnTwitterSignIn.ContextMenuStrip = null;
					btnTwitterSignIn.Text = "Sign out";
					lblTwitterTokenStatus.ForeColor = Color.Green;
                    lblTwitterTokenStatus.Text = $"{screenName} ({twitterCredentials.AccessToken}...)";
				}
			} catch (Exception e) {
                lblTwitterTokenStatus.ForeColor = Color.Red;
                lblTwitterTokenStatus.Text = $"{e.Message} ({twitterCredentials.AccessToken}...)";
			}
		}

        private async void UpdateFurAffinityTokenLabel() {
            if (Settings.FurAffinity?.a != null && Settings.FurAffinity?.b != null) {
                btnFurAffinitySignIn.Text = "Sign out";
                var fa = new FurAffinityMinimalSourceWrapper(Settings.FurAffinity.a, Settings.FurAffinity.b, false);
                try {
                    string username = await fa.AsISourceWrapper().WhoamiAsync();
                    lblfurAffinityUsername2.ForeColor = Color.Green;
                    lblfurAffinityUsername2.Text = $"{username} ({Settings.FurAffinity.a}..., {Settings.FurAffinity.b}...)";
                } catch (Exception e) {
                    lblfurAffinityUsername2.ForeColor = Color.Red;
                    lblfurAffinityUsername2.Text = $"{e.Message} ({Settings.FurAffinity.a}..., {Settings.FurAffinity.b}...)";
                }
            } else {
                btnFurAffinitySignIn.Text = "Sign in";
                lblfurAffinityUsername2.ForeColor = SystemColors.WindowText;
                lblfurAffinityUsername2.Text = "Not signed in";
            }
        }

        private async void UpdateFlickrTokenLabel() {
            if (Settings.Flickr?.TokenKey != null && Settings.Flickr?.TokenSecret != null) {
                btnFlickrSignIn.Text = "Sign out";
                try {
                    IPagedSourceWrapper<int> flickr = new FlickrSourceWrapper(OAuthConsumer.Flickr.KEY, OAuthConsumer.Flickr.SECRET, Settings.Flickr.TokenKey, Settings.Flickr.TokenSecret);
                    string username = await flickr.WhoamiAsync();
                    lblFlickrTokenStatus.ForeColor = Color.Green;
                    lblFlickrTokenStatus.Text = $"{username} ({Settings.Flickr.TokenKey}...)";
                } catch (Exception e) {
                    lblFlickrTokenStatus.ForeColor = Color.Red;
                    lblFlickrTokenStatus.Text = $"{e.Message} ({Settings.Flickr.TokenKey}...)";
                }
            } else {
                btnFlickrSignIn.Text = "Sign in";
                lblFlickrTokenStatus.ForeColor = SystemColors.WindowText;
                lblFlickrTokenStatus.Text = "Not signed in";
            }
		}

		private void UpdatePixivTokenLabel() {
			if (Settings.Pixiv.Username != null) {
				btnPixivSignIn.Text = "Sign out";
				try {
					lblPixivUsername2.ForeColor = Color.Green;
					lblPixivUsername2.Text = Settings.Pixiv.Username;
				} catch (Exception e) {
					lblPixivUsername2.ForeColor = Color.Red;
					lblPixivUsername2.Text = e.Message;
				}
			} else {
				btnPixivSignIn.Text = "Sign in";
				lblPixivUsername2.Text = "Not signed in";
			}
		}

		private async void UpdateFurryNetworkTokenLabel() {
			if (Settings.FurryNetwork.RefreshToken != null) {
				btnFurryNetworkSignIn.Text = "Sign out";
				try {
					var client = new FurryNetworkClient(Settings.FurryNetwork.RefreshToken);
					var user = await client.GetUserAsync();
					lblFurryNetworkTokenStatus.ForeColor = Color.Green;
					lblFurryNetworkTokenStatus.Text = user.DefaultCharacter.Name;
				} catch (Exception e) {
					lblFurryNetworkTokenStatus.ForeColor = Color.Red;
					lblFurryNetworkTokenStatus.Text = e.Message;
				}
			} else {
				btnFurryNetworkSignIn.Text = "Sign in";
				lblFurryNetworkTokenStatus.Text = "Not signed in";
			}
		}
	}
}
