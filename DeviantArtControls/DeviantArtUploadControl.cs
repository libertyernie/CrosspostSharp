using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DeviantArtControls {
    public partial class DeviantArtUploadControl : UserControl {
        private DeviantArtCategoryBrowser.Category _selectedCategory;
        public DeviantArtCategoryBrowser.Category SelectedCategory {
            get {
                return _selectedCategory;
            }
            set {
                _selectedCategory = value;
                txtCategory.Text = string.Join(" > ", value?.NamePath ?? new[] { "None" });
            }
        }

        private IEnumerable<DeviantArtFolderSelectionForm.Folder> _selectedFolders;
        public IEnumerable<DeviantArtFolderSelectionForm.Folder> SelectedFolders {
            get {
                return _selectedFolders;
            }
            set {
                _selectedFolders = value;
                txtGalleryFolders.Text = string.Join(", ", value.Select(f => f.Name));
            }
        }

        public DeviantArtUploadControl() {
            InitializeComponent();

            radNone.CheckedChanged += MatureChanged;
            radModerate.CheckedChanged += MatureChanged;
            radStrict.CheckedChanged += MatureChanged;
        }

        private void MatureChanged(object sender, EventArgs e) {
            grpMatureClassification.Enabled = radNone.Checked;
        }

        private void btnCategory_Click(object sender, EventArgs e) {
            using (var f = new DeviantArtCategoryBrowser()) {
                f.InitialCategory = SelectedCategory;
                if (f.ShowDialog() == DialogResult.OK) {
                    SelectedCategory = f.SelectedCategory;
                }
            }
        }

        private void btnGalleryFolders_Click(object sender, EventArgs e) {
            try {
                using (var form = new DeviantArtFolderSelectionForm()) {
                    if (form.ShowDialog() == DialogResult.OK) {
                        SelectedFolders = form.SelectedFolders;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType()}, {ex.GetType()}");
            }
        }
    }
}
