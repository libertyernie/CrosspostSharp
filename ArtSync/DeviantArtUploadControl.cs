using System;
using System.Linq;
using System.Windows.Forms;

namespace ArtSync {
    public partial class DeviantArtUploadControl : UserControl {
        private DeviantArtCategoryBrowser.Category _selectedCategory;
        public DeviantArtCategoryBrowser.Category SelectedCategory {
            get {
                return _selectedCategory;
            }
            set {
                _selectedCategory = value;
                txtCategory.Text = string.Join(" > ", value?.NamePath ?? Enumerable.Empty<string>());
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
    }
}
