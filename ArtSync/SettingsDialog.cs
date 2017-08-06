using ArtSourceWrapper;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
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

namespace ArtSync {
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

			Settings.Defaults.IncludeWeasylTag = chkWeasylSubmitIdTag.Checked;

			Settings.Save();
        }

        private async void btnDeviantArtSignIn_Click(object sender, EventArgs e) {
            try {
                if (string.Equals("Sign out", btnDeviantArtSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
                    await DeviantArtWrapper.LogoutAsync();
                    Settings.DeviantArt.RefreshToken = null;
                } else {
                    var dA = new DeviantArtWrapper(OAuthConsumer.DeviantArt.CLIENT_ID, OAuthConsumer.DeviantArt.CLIENT_SECRET);
                    Settings.DeviantArt.RefreshToken = await dA.UpdateTokens();
                }
                UpdateDeviantArtTokenLabel();
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

		private void FillForm() {
			txtWeasylAPIKey.Text = Settings.Weasyl.APIKey ?? "";

			txtBlogName.Text = Settings.Tumblr.BlogName ?? "";
			chkSidePadding.Checked = Settings.Tumblr.AutoSidePadding;

			txtHeader.Text = Settings.Defaults.HeaderHTML ?? "";
			txtFooter.Text = Settings.Defaults.FooterHTML ?? "";
			txtTags.Text = Settings.Defaults.Tags;

			chkWeasylSubmitIdTag.Checked = Settings.Defaults.IncludeWeasylTag;

            UpdateDeviantArtTokenLabel();
            UpdateTumblrTokenLabel();
			UpdateTwitterTokenLabel();
        }

        private void UpdateDeviantArtTokenLabel() {
            if (Settings.DeviantArt.RefreshToken != null) {
                btnDeviantArtSignIn.Text = "Sign out";
                lblDeviantArtTokenStatus.Text = Settings.DeviantArt.RefreshToken;
            } else {
				btnDeviantArtSignIn.Text = "Sign in";
                lblDeviantArtTokenStatus.Text = "Not signed in";
			}
        }

        private void UpdateTumblrTokenLabel() {
			Token token = Settings.TumblrToken;
			if (token.IsValid) {
				btnTumblrSignIn.ContextMenuStrip = null;
				btnTumblrSignIn.Text = "Sign out";
				using (TumblrClient client = new TumblrClientFactory().Create<TumblrClient>(
					OAuthConsumer.Tumblr.CONSUMER_KEY,
					OAuthConsumer.Tumblr.CONSUMER_SECRET,
					token
				)) {
					try {
						Task<UserInfo> t = client.GetUserInfoAsync();
						t.Wait();
						lblTokenStatus.ForeColor = Color.Green;
						lblTokenStatus.Text = string.Format("{0} ({1}...)", t.Result.Name, token.Key.Substring(0, 8));
					} catch (AggregateException e) {
						lblTokenStatus.ForeColor = Color.Red;
						lblTokenStatus.Text = string.Format("{0} ({1}...)", string.Join(", ", e.InnerExceptions.Select(x => x.Message)), token.Key.Substring(0, 8));
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
                lblInkbunnyTokenStatus.ForeColor = Color.Green;
                lblInkbunnyTokenStatus.Text = "";
            } else {
                btnInkbunnySignIn.Text = "Sign out";
                lblInkbunnyTokenStatus.ForeColor = SystemColors.WindowText;
                lblInkbunnyTokenStatus.Text = Settings.Inkbunny.Sid;
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
					lblTwitterTokenStatus.Text = string.Format("{0} ({1}...)", screenName, twitterCredentials.AccessToken.Substring(0, 8));
				}
			} catch (Exception e) {
				lblTokenStatus.ForeColor = Color.Red;
				lblTokenStatus.Text = string.Format("{0} ({1}...)", e.Message, twitterCredentials.AccessToken.Substring(0, 8));
			}
		}
    }
}
