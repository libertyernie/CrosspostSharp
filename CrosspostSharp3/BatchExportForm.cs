using Newtonsoft.Json;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class BatchExportForm : Form {
		public BatchExportForm(IEnumerable<ISourceWrapper> wrappers) {
			InitializeComponent();
			foreach (var w in wrappers) listBox1.Items.Add(w);
		}

		public IEnumerable<ISourceWrapper> SelectedWrappers {
			get {
				foreach (var o in listBox1.SelectedItems) {
					if (o is ISourceWrapper w) yield return w;
				}
			}
		}

		private async void btnOk_Click(object sender, EventArgs e) {
			if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) {
				return;
			}

			panel1.Enabled = false;

			progressBar1.Maximum = (int)numericUpDown1.Value;
			progressBar1.Value = 0;

			try {
				if (SelectedWrappers.Count() != 1) {
					throw new NotImplementedException();
				}

				var wrapper = SelectedWrappers.Single();

				var posts = await wrapper.FetchAllAsync((int)numericUpDown1.Value);

				foreach (var submission in posts) {
					progressBar1.Value++;
					var artworkData = await ArtworkData.DownloadAsync(submission);
					string imagePath = Path.Combine(folderBrowserDialog1.SelectedPath, artworkData.GetFileName());

					if (chkExportImage.Checked) {
						File.WriteAllBytes(imagePath, artworkData.data);
					}
					if (chkExportCps.Checked) {
						File.WriteAllText(imagePath + ".cps", JsonConvert.SerializeObject(artworkData, Formatting.Indented));
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			progressBar1.Value = 0;
			panel1.Enabled = true;
		}
	}
}
