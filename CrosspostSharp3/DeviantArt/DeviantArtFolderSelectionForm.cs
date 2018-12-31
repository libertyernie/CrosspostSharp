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
		public IEnumerable<DeviantArtFs.Gallery.Folder> InitialFolders { get; set; }

		private readonly IDeviantArtAccessToken _token;
		private List<DeviantArtFs.Gallery.Folder> _selectedFolders;
		public IEnumerable<DeviantArtFs.Gallery.Folder> SelectedFolders => _selectedFolders;

		public DeviantArtFolderSelectionForm(IDeviantArtAccessToken token) {
			InitializeComponent();
			_token = token;
			_selectedFolders = new List<DeviantArtFs.Gallery.Folder>();
		}

		private async void DeviantArtFolderSelectionForm_Load(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var resp = await DeviantArtFs.Gallery.Folders.ExecuteAsync(_token, new DeviantArtFs.Gallery.FoldersRequest { });

				int skip = 0;
				while (resp.Results.Any()) {
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
					skip += resp.Results.Count();
					resp = await DeviantArtFs.Gallery.Folders.ExecuteAsync(_token, new DeviantArtFs.Gallery.FoldersRequest {
						Offset = skip
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
