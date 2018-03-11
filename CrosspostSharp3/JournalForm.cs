using CrosspostSharpJournal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
					sources.Add(new FurAffinityJournalSource(x.a, x.b));
				}
				_source = new MetaJournalSource(sources);
				for (int i = 0; i < 15; i++) {
					while (!_source.Cache.Skip(i).Any()) await _source.FetchAsync();
					lstSource.Items.Add(_source.Cache.Skip(i).First());
				}
			};

			webDescription.Navigate("about:blank");
			webDescription.Document.Write($"<html><head></head><body></body></html>");
			webDescription.Document.Body.SetAttribute("contenteditable", "true");
			webTeaser.Navigate("about:blank");
			webTeaser.Document.Write($"<html><head></head><body></body></html>");
			webTeaser.Document.Body.SetAttribute("contenteditable", "true");
		}

		private void lstSource_SelectedIndexChanged(object sender, EventArgs e) {
			var j = lstSource.SelectedItem as IJournalWrapper;
			lblTimestamp.Text = (j?.Timestamp)?.ToLongDateString() ?? "";
			txtTitle.Text = j?.Title ?? "";
			webDescription.Document.Body.InnerHtml = j?.HTMLDescription ?? "";
			webTeaser.Document.Body.InnerHtml = "";
		}
	}
}
