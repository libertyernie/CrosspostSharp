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
				var wrapper = new DeviantArtJournalSiteWrapper();
				await wrapper.FetchAsync();
				foreach (var j in wrapper.Cache) {
					lstSource.Items.Add(j);
				}
			};
		}

		private void lstSource_SelectedIndexChanged(object sender, EventArgs e) {
			var j = lstSource.SelectedItem as IJournalWrapper;
			lblTimestamp.Text = (j?.Timestamp)?.ToLongDateString() ?? "";
			txtTitle.Text = j?.Title ?? "";
			textBox1.Text = j?.HTMLDescription ?? "";
			textBox2.Text = "";
		}
	}
}
