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
		public IEnumerable<IBclDeviantArtGalleryFolder> InitialFolders { get; set; }

		private readonly IDeviantArtAccessToken _token;
		private List<IBclDeviantArtGalleryFolder> _selectedFolders;
		public IEnumerable<IBclDeviantArtGalleryFolder> SelectedFolders => _selectedFolders;

		public DeviantArtFolderSelectionForm(IDeviantArtAccessToken token) {
			InitializeComponent();
			_token = token;
			_selectedFolders = new List<IBclDeviantArtGalleryFolder>();
		}

		private async void DeviantArtFolderSelectionForm_Load(object sender, EventArgs e) {
			try {
				this.Enabled = false;

				var list = await DeviantArtFs.Requests.Gallery.GalleryFolders.ToArrayAsync(_token, new DeviantArtFs.Requests.Gallery.GalleryFoldersRequest { });

				foreach (var f in list) {
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

				this.Enabled = true;
			} catch (Exception ex) {
				MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType().Name}: {ex.GetType().Name}");
				this.Close();
			}
		}
	}
}
