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
                txtCategory.Text = value == null
                    ? ""
                    : string.Join(" > ", value?.NamePath ?? new[] { "None" });
            }
        }

        private IEnumerable<DeviantArtFolderSelectionForm.Folder> _selectedFolders;
        public IEnumerable<DeviantArtFolderSelectionForm.Folder> SelectedFolders {
            get {
                return _selectedFolders;
            }
            set {
                _selectedFolders = value;
                txtGalleryFolders.Text = value == null
                    ? ""
                    : string.Join(", ", value.Select(f => f.Name));
            }
        }

        public DeviantArtUploadControl() {
            InitializeComponent();

            radNone.CheckedChanged += MatureChanged;
            radModerate.CheckedChanged += MatureChanged;
            radStrict.CheckedChanged += MatureChanged;

            ddlLicense.SelectedIndex = 0;
            ddlSharing.SelectedIndex = 0;
        }

        private void MatureChanged(object sender, EventArgs e) {
            grpMatureClassification.Enabled = !radNone.Checked;
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
            if (chkSubmissionPolicy.Checked == false || chkTermsOfService.Checked == false) {
                MessageBox.Show("Before submitting to DeviantArt, you must agree to the Submission Policy and the Terms of Service.");
            }

            try {
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
                    OriginalUrl = "https://www.example.com/hello",
                    Tags = new HashSet<string>(txtTags.Text.Replace("#", "").Replace(",", "").Split(' ').Where(s => s != "")),
                    Title = txtTitle.Text
                }.ExecuteAsync();
                if (r1.IsError) {
                    throw new Exception("Could not post to sta.sh: " + r1.ErrorText);
                }
                if (!string.IsNullOrEmpty(r1.Object.Error)) {
                    throw new Exception("Could not post to sta.sh: " + r1.Object.ErrorDescription);
                }

                var sharingStr = ddlSharing.SelectedItem?.ToString();
                var sharing = sharingStr == "Show share buttons" ? PublishRequest.SharingOption.Allow
                        : sharingStr == "Hide share buttons" ? PublishRequest.SharingOption.HideShareButtons
                        : sharingStr == "Hide & require login to view" ? PublishRequest.SharingOption.HideAndMembersOnly
                        : throw new Exception("Unrecognized ddlSharing.SelectedItem");

                var r2 = await new PublishRequest {
                    IsMature = !radNone.Checked,
                    MatureLevel = radStrict.Checked
                        ? PublishRequest.LevelOFMature.Strict
                        : PublishRequest.LevelOFMature.Moderate,
                    MatureClassification = classifications,
                    AgreeSubmission = chkSubmissionPolicy.Checked,
                    AgreeTos = chkTermsOfService.Checked,
                    CatPath = SelectedCategory?.CategoryPath,
                    AllowComments = chkAllowComments.Checked,
                    RequestCritique = chkRequestCritique.Checked,
                    Sharing = sharing,
                    LicenseCreativeCommons = ddlLicense.SelectedText.Contains("CC-"),
                    LicenseComercial = !ddlLicense.SelectedText.Contains("-NC"),
                    LicenseModify = ddlLicense.SelectedText.Contains("-ND") ? PublishRequest.LicenseModifyOption.No
                        : ddlLicense.SelectedText.Contains("-SA") ? PublishRequest.LicenseModifyOption.Share
                        : PublishRequest.LicenseModifyOption.Yes,
                    GalleryIds = new HashSet<string>(SelectedFolders == null
                        ? Enumerable.Empty<string>()
                        : SelectedFolders.Select(f => f.FolderId)),
                    AllowFreeDownload = chkAllowFreeDownload.Checked,
                    ItemId = r1.Object.ItemId
                }.ExecuteAsync();
                if (r2.IsError) {
                    throw new Exception("Posted to sta.sh but could not post to DeviantArt: " + r2.ErrorText + Environment.NewLine);
                }
                if (!string.IsNullOrEmpty(r2.Object.Error)) {
                    throw new Exception("Posted to sta.sh but could not post to DeviantArt: " + r2.Object.ErrorDescription);
                }
                MessageBox.Show(r2.Object.Url);
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, $"{GetType()} {ex.GetType()}");
            }
        }

        private void ShowHTMLDialog(DeviantartApi.Requests.Request<DeviantartApi.Objects.Information> request, string fallbackUrl) {
            using (var form = new Form()) {
                form.Width = 400;
                form.Height = 600;
                var browser = new WebBrowser {
                    Dock = DockStyle.Fill
                };
                form.Controls.Add(browser);
                form.Load += async (x, y) => {
                    browser.Navigate("about:blank");
                    try {
                        var result = await request.ExecuteAsync();
                        if (result.IsError) {
                            throw new Exception(result.ErrorText);
                        }
                        if (!string.IsNullOrEmpty(result.Object.Error)) {
                            throw new Exception(result.Object.ErrorDescription);
                        }
                        browser.Document.Write(result.Object.Text);
                    } catch (Exception) {
                        browser.Navigate(fallbackUrl);
                    }
                };
                form.ShowDialog(this);
            }
        }

        private void lnkSubmissionPolicy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ShowHTMLDialog(new DeviantartApi.Requests.Data.SubmissionRequest(), "https://about.deviantart.com/policy/submission/");
        }

        private void lnkTermsOfService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ShowHTMLDialog(new DeviantartApi.Requests.Data.TermsOfServiceRequest(), "https://about.deviantart.com/policy/service/");
        }
    }
}
