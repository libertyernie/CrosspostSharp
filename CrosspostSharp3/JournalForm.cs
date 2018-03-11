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
		public JournalForm() {
			InitializeComponent();

			Shown += async (o, a) => {
				Settings s = Settings.Load();

				var wrappers = new List<IJournalSource>();
				if (s.DeviantArt.RefreshToken != null) {
					wrappers.Add(new DeviantArtJournalSource());
				}
				foreach (var x in s.FurAffinity) {
					wrappers.Add(new FurAffinityJournalSource(x.a, x.b));
				}
				await wrappers.Last().FetchAsync();
				foreach (var j in wrappers.Last().Cache) {
					lstSource.Items.Add(j);
				}
			};
		}

		private void lstSource_SelectedIndexChanged(object sender, EventArgs e) {
			var j = lstSource.SelectedItem as IJournalWrapper;
			lblTimestamp.Text = (j?.Timestamp)?.ToLongDateString() ?? "";
			txtTitle.Text = j?.Title ?? "";
			// TODO fix this stuff below
			webDescription.Navigate("about:blank");
			webDescription.Document.Write($"<html><head></head><body>{j.HTMLDescription ?? ""}</body></html>");
			webDescription.Document.Body.SetAttribute("contenteditable", "true");
			webTeaser.Navigate("about:blank");
			webTeaser.Document.Write($"<html><head></head><body></body></html>");
			webTeaser.Document.Body.SetAttribute("contenteditable", "true");
		}
	}
}
