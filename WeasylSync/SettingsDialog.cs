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
			FillForm();
		}

		private void btnSave_Click(object sender, EventArgs e) {
			Settings.Weasyl.APIKey = txtWeasylAPIKey.Text;
			if (Settings.Weasyl.APIKey == "") Settings.Weasyl.APIKey = null;

			Settings.Tumblr.BlogName = txtBlogName.Text;
			Settings.Tumblr.Header = txtHeader.Text;
			Settings.Tumblr.Footer = txtFooter.Text;
			Settings.Tumblr.Tags = txtTags.Text;

			Settings.Save();
		}

		private void btnTumblrSignIn_Click(object sender, EventArgs e) {
			if (string.Equals("Sign out", btnTumblrSignIn.Text, StringComparison.InvariantCultureIgnoreCase)) {
				Settings.TumblrToken = null;
			} else {
				Settings.TumblrToken = TumblrKey.Obtain(OAuthConsumer.CONSUMER_KEY, OAuthConsumer.CONSUMER_SECRET);
			}
			UpdateTokenLabel();
		}

		private void FillForm() {
			txtWeasylAPIKey.Text = Settings.Weasyl.APIKey ?? "";

			txtBlogName.Text = Settings.Tumblr.BlogName ?? "";
			txtHeader.Text = Settings.Tumblr.Header ?? "";
			txtFooter.Text = Settings.Tumblr.Footer ?? "";
			txtTags.Text = Settings.Tumblr.Tags;

			UpdateTokenLabel();
		}

		private void UpdateTokenLabel() {
			Token token = Settings.TumblrToken;
			if (token.IsValid) {
				btnTumblrSignIn.ContextMenuStrip = null;
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
				btnTumblrSignIn.ContextMenuStrip = menuSignIn;
				btnTumblrSignIn.Text = "Sign in";
				lblTokenStatus.Text = "Not signed in";
			}
		}

		private void menuItemPrivate_Click(object sender, EventArgs e) {
			btnTumblrSignIn_Click(sender, e);
		}

		private void menuItemIECookies_Click(object sender, EventArgs e) {
			IECookiePersist.Suppress(false);
			btnTumblrSignIn_Click(sender, e);
			IECookiePersist.Suppress(true);
		}
	}
}
