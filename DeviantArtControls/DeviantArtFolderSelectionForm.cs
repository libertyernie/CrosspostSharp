using DeviantartApi.Objects.SubObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviantArtControls {
    public partial class DeviantArtFolderSelectionForm : Form {
        public class Folder {
            public string FolderId;
            public string Name;
        }

        public IEnumerable<Folder> InitialFolders { get; set; }

        private List<GalleryFolder> _selectedFolders;
        public IEnumerable<Folder> SelectedFolders => _selectedFolders.Select(f => new Folder {
            FolderId = f.FolderId,
            Name = f.Name
        });

        public DeviantArtFolderSelectionForm() {
            InitializeComponent();
            _selectedFolders = new List<GalleryFolder>();
        }

        private async void DeviantArtFolderSelectionForm_Load(object sender, EventArgs e) {
            try {
                this.Enabled = false;

                var list = new List<GalleryFolder>();

                var req = new DeviantartApi.Requests.Gallery.FoldersRequest {
                    CalculateSize = false,
                    ExtPreload = false
                };

                while (true) {
                    var resp = await req.GetNextPageAsync();
                    foreach (var f in resp.Object.Results) {
                        var chk = new CheckBox {
                            AutoSize = true,
                            Text = f.Name,
                            Checked = InitialFolders?.Any(f2 => f.FolderId == f2.FolderId) == true
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
                    if (!resp.Object.HasMore) break;
                }

                this.Enabled = true;
            } catch (Exception ex) {
                MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType().Name}: {ex.GetType().Name}");
                this.Close();
            }
        }
    }
}
