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
		private Settings settings;

		public SettingsDialog(Settings settings) {
			InitializeComponent();
			this.settings = settings;

			txtWeasylUsername.Text = settings.Weasyl.Username ?? "";
			txtWeasylAPIKey.Text = settings.Weasyl.APIKey ?? "";

			txtBlogName.Text = settings.Tumblr.BlogName ?? "";
			txtFooter.Text = settings.Tumblr.Footer ?? "";
			chkFooterIsLink.Checked = settings.Tumblr.FooterIsLink;
			txtTags.Text = settings.Tumblr.Tags;

			bool tokenValid = settings.TumblrToken.IsValid;
			lblTokenStatus.Text = tokenValid ? "Signed in" : "Not signed in";
			lblTokenStatus.ForeColor = tokenValid ? Color.DarkGreen : Color.DarkRed;
			btnTumblrSignIn.Text = tokenValid ? "Sign out" : "Sign in";
		}

		private void btnSave_Click(object sender, EventArgs e) {
			settings.Weasyl.Username = txtWeasylUsername.Text;
			settings.Weasyl.APIKey = txtWeasylAPIKey.Text;
			if (settings.Weasyl.APIKey == "") settings.Weasyl.APIKey = null;

			settings.Tumblr.BlogName = txtBlogName.Text;
			settings.Tumblr.Footer = txtFooter.Text;
			settings.Tumblr.FooterIsLink = chkFooterIsLink.Checked;
			settings.Tumblr.Tags = txtTags.Text;

			settings.Save();
		}

		private void chkFooterIsLink_CheckedChanged(object sender, EventArgs e) {
			txtFooter.Font = new Font(txtFooter.Font, chkFooterIsLink.Checked ? FontStyle.Underline : FontStyle.Regular);
			txtFooter.ForeColor = chkFooterIsLink.Checked ? Color.Blue : SystemColors.WindowText;
		}
	}
}
