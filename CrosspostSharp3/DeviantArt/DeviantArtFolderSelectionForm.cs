using DeviantArtFs;
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
	public partial class DeviantArtFolderSelectionForm : Form {
		public IEnumerable<IDeviantArtFolder> InitialFolders { get; set; }

		private readonly IDeviantArtAccessToken _token;
		private List<IDeviantArtFolder> _selectedFolders;
		public IEnumerable<IDeviantArtFolder> SelectedFolders => _selectedFolders;

		public DeviantArtFolderSelectionForm(IDeviantArtAccessToken token) {
			InitializeComponent();
			_token = token;
			_selectedFolders = new List<IDeviantArtFolder>();
		}

		private async void DeviantArtFolderSelectionForm_Load(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var resp = await DeviantArtFs.Requests.Gallery.Folders.ExecuteAsync(_token, new DeviantArtFs.Requests.Gallery.FoldersRequest { });

				while (true) {
					foreach (var f in resp.Results) {
						var chk = new CheckBox {
							AutoSize = true,
							Text = f.Name,
							Checked = InitialFolders?.Any(f2 => f.Folderid == f2.Folderid) == true
						};
						chk.CheckedChanged += (o, ea) => {
							if (chk.Checked) {
								_selectedFolders.Add(f);
							} else {
								_selectedFolders.Remove(f);
							}
						};
						flowLayoutPanel1.Controls.Add(chk);
					}

					if (!resp.HasMore) break;

					resp = await DeviantArtFs.Requests.Gallery.Folders.ExecuteAsync(_token, new DeviantArtFs.Requests.Gallery.FoldersRequest {
						Offset = resp.NextOffset.Value
					});
				}

				this.Enabled = true;
			} catch (Exception ex) {
				MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType().Name}: {ex.GetType().Name}");
				this.Close();
			}
		}
	}
}
