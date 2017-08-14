using DeviantartApi.Requests.Stash;
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

        private async void btnPublish_Click(object sender, EventArgs e) {
            var classifications = new HashSet<PublishRequest.ClassificationOfMature>();
            if (chkNudity.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Nudity);
            if (chkSexual.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Sexual);
            if (chkGore.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Gore);
            if (chkLanguage.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Language);
            if (chkIdeology.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Ideology);

            var r1 = await new SubmitRequest {
                ArtistComments = txtArtistComments.Text,
                Data = System.IO.File.ReadAllBytes(@"C:\Windows\Web\Wallpaper\Theme2\img8.jpg"),
                IsDirty = false,
                OriginalUrl = "https://www.example.com/hello"
            }.ExecuteAsync();
            var r2 = await new PublishRequest {
                IsMature = !radNone.Checked,
                MatureLevel = radStrict.Checked
                    ? PublishRequest.LevelOFMature.Strict
                    : PublishRequest.LevelOFMature.Moderate,
                MatureClassification = classifications,
                AgreeSubmission = false,
                AgreeTos = false,
                CatPath = SelectedCategory.CategoryPath,
                AllowComments = chkAllowComments.Checked,
                RequestCritique = chkRequestCritique.Checked,
                Sharing = ddlSharing.SelectedText == "Show share buttons" ? PublishRequest.SharingOption.Allow
                    : ddlSharing.SelectedText == "Hide share buttons" ? PublishRequest.SharingOption.HideShareButtons
                    : ddlSharing.SelectedText == "Hide & require login to view" ? PublishRequest.SharingOption.HideAndMembersOnly
                    : throw new Exception("Unrecognized ddlSharing.SelectedText"),
                LicenseCreativeCommons = ddlLicense.SelectedText.Contains("CC-"),
                LicenseComercial = !ddlLicense.SelectedText.Contains("-NC"),
                LicenseModify = ddlLicense.SelectedText.Contains("-ND") ? PublishRequest.LicenseModifyOption.No
                    : ddlLicense.SelectedText.Contains("-SA") ? PublishRequest.LicenseModifyOption.Share
                    : PublishRequest.LicenseModifyOption.Yes,
                GalleryIds = new HashSet<string>(SelectedFolders.Select(f => f.FolderId)),
                AllowFreeDownload = chkAllowFreeDownload.Checked,
                ItemId = r1.Object.ItemId.ToString()
            }.ExecuteAsync();
        }
    }
}
