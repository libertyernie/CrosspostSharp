using CrosspostSharpJournal;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class JournalForm : Form {
		private IJournalSource _source;

		public JournalForm() {
			InitializeComponent();

			Shown += async (o, a) => {
				Settings s = Settings.Load();

				var sources = new List<IJournalSource>();
				if (s.DeviantArt.RefreshToken != null) {
					sources.Add(new DeviantArtJournalSource());
				}
				foreach (var x in s.FurAffinity) {
					var y = new FurAffinityJournalSource(x.a, x.b);
					sources.Add(y);
				}
				foreach (var x in s.Tumblr) {
					var y = new TumblrJournalSource(
						new TumblrClientFactory().Create<TumblrClient>(
							OAuthConsumer.Tumblr.CONSUMER_KEY,
							OAuthConsumer.Tumblr.CONSUMER_SECRET,
							new Token(x.tokenKey, x.tokenSecret)),
						x.blogName);
					sources.Add(y);
					lstDestination.Items.Add(y);
				}
				_source = new MetaJournalSource(sources);
				for (int i = 0; i < 15; i++) {
					while (!_source.Cache.Skip(i).Any() && !_source.IsEnded) await _source.FetchAsync();
					var j = _source.Cache.Skip(i).FirstOrDefault();
					if (j == null) break;
					lstSource.Items.Add(j);
				}
			};
		}

		private void lstSource_SelectedIndexChanged(object sender, EventArgs e) {
			var j = lstSource.SelectedItem as IJournalWrapper;
			lblTimestamp.Text = (j?.Timestamp)?.ToLongDateString() ?? "";
			txtTitle.Text = j?.Title ?? "";
			txtBody.Text = HtmlConversion.ConvertHtmlToText(j?.HTMLDescription ?? "");
			txtTeaser.Text = "";
		}

		private void lstDestination_SelectedIndexChanged(object sender, EventArgs e) {
			txtTeaser.Enabled = false;
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			var destination = lstDestination.SelectedItem as IJournalDestination;
			if (destination == null) {
				MessageBox.Show(this, "Please select a destination from the menu in the lower-left.", Text);
				return;
			}

			btnPost.Enabled = false;
			try {
				await destination.PostAsync(txtTitle.Text, txtBody.Text, txtTeaser.Text);
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btnPost.Enabled = true;
		}
	}
}
