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

namespace WeasylSync {
	public partial class SettingsDialog : Form {
		public Settings Settings { get; private set; }

		public SettingsDialog(Settings settings) {
			InitializeComponent();
			this.Settings = settings.Copy();

			txtWeasylUsername.Text = settings.Weasyl.Username ?? "";
			txtWeasylAPIKey.Text = settings.Weasyl.APIKey ?? "";

			txtBlogName.Text = settings.Tumblr.BlogName ?? "";
			txtFooter.Text = settings.Tumblr.Footer ?? "";
			chkFooterIsLink.Checked = settings.Tumblr.FooterIsLink;
			txtTags.Text = settings.Tumblr.Tags;

			UpdateTokenLabel();
		}

		private void btnSave_Click(object sender, EventArgs e) {
			Settings.Weasyl.Username = txtWeasylUsername.Text;
			Settings.Weasyl.APIKey = txtWeasylAPIKey.Text;
			if (Settings.Weasyl.APIKey == "") Settings.Weasyl.APIKey = null;

			Settings.Tumblr.BlogName = txtBlogName.Text;
			Settings.Tumblr.Footer = txtFooter.Text;
			Settings.Tumblr.FooterIsLink = chkFooterIsLink.Checked;
			Settings.Tumblr.Tags = txtTags.Text;

			Settings.Save();
		}

		private void chkFooterIsLink_CheckedChanged(object sender, EventArgs e) {
			txtFooter.Font = new Font(txtFooter.Font, chkFooterIsLink.Checked ? FontStyle.Underline : FontStyle.Regular);
			txtFooter.ForeColor = chkFooterIsLink.Checked ? Color.Blue : SystemColors.WindowText;
		}

		private void btnTumblrSignIn_Click(object sender, EventArgs e) {
			if (string.Equals("Sign out", btnTumblrSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
				Settings.TumblrToken = null;
			} else {
				Settings.TumblrToken = TumblrKey.Obtain(OAuthConsumer.CONSUMER_KEY, OAuthConsumer.CONSUMER_SECRET);
			}
			UpdateTokenLabel();
		}

		private void UpdateTokenLabel() {
			Token token = Settings.TumblrToken;
			if (token.IsValid) {
				btnTumblrSignIn.Text = "Sign out";
				using (TumblrClient client = new TumblrClientFactory().Create<TumblrClient>(
				OAuthConsumer.CONSUMER_KEY,
				OAuthConsumer.CONSUMER_SECRET,
				token)) {
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
	}
}
