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
	public partial class PostAlreadyExistsDialog : Form {
		public static class Result {
			public const DialogResult Replace = DialogResult.Yes;
			public const DialogResult AddNew = DialogResult.No;
		}

		public PostAlreadyExistsDialog(string tag, string tumblrUrl) {
			InitializeComponent();

			lblMessage.Text = lblMessage.Text.Replace("{TAG}", tag);
			lnkTumblrPost.Text = tumblrUrl;
		}

		private void lnkTumblrPost_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(lnkTumblrPost.Text);
		}
	}
}
