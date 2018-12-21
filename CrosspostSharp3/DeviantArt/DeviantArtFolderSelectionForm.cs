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
        public class Folder {
            public Guid FolderId;
            public string Name;
        }

        public IEnumerable<Folder> InitialFolders { get; set; }

		private readonly IDeviantArtAccessToken _token;
		private List<KeyValuePair<Guid, string>> _selectedFolders;
		public IEnumerable<Folder> SelectedFolders =>
			_selectedFolders.Select(f => new Folder {
				FolderId = f.Key,
				Name = f.Value
			});

		public DeviantArtFolderSelectionForm(IDeviantArtAccessToken token) {
            InitializeComponent();
			_token = token;
            _selectedFolders = new List<KeyValuePair<Guid, string>>();
        }

        private async void DeviantArtFolderSelectionForm_Load(object sender, EventArgs e) {
            try {
				this.Enabled = false;

				var resp = await DeviantArtFs.Gallery.Folders.ExecuteAsync(_token, new DeviantArtFs.Gallery.GalleryFoldersRequest { });

				int skip = 0;
				while (resp.Any()) {
					foreach (var f in resp) {
						var chk = new CheckBox {
							AutoSize = true,
							Text = f.Value,
							Checked = InitialFolders?.Any(f2 => f.Key == f2.FolderId) == true
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
					skip += resp.Count;
					resp = await DeviantArtFs.Gallery.Folders.ExecuteAsync(_token, new DeviantArtFs.Gallery.GalleryFoldersRequest {
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
