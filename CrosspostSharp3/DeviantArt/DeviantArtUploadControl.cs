using DeviantArtFs;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
    public partial class DeviantArtUploadControl : UserControl {
		public delegate void DeviantArtUploadedHandler(string url);
		public event DeviantArtUploadedHandler Uploaded;

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

		private SavedPhotoPost _downloaded;
		private long? _stashItemId;

        private string _originalUrl;

		public string UploadedUrl { get; private set; }

		private readonly DeviantArtClient _client;

		public DeviantArtUploadControl(IDeviantArtAccessToken token) {
            InitializeComponent();
			_client = new DeviantArtClient(token);

			radNone.CheckedChanged += MatureChanged;
            radModerate.CheckedChanged += MatureChanged;
            radStrict.CheckedChanged += MatureChanged;

            ddlLicense.SelectedIndex = 0;
            ddlSharing.SelectedIndex = 0;
        }

        public void SetSubmission(SavedPhotoPost post, long? stashItemId) {
			_downloaded = post;
			_stashItemId = stashItemId;

            txtTitle.Text = post.title ?? "";
            txtArtistComments.Text = post.description ?? "";
            txtTags.Text = string.Join(" ", post.tags?.Select(s => $"#{s}") ?? Enumerable.Empty<string>());
            if (post.mature) {
                radStrict.Checked = true;
            } else {
                radNone.Checked = true;
            }
            _originalUrl = post.url;
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
                using (var form = new DeviantArtFolderSelectionForm(_client)) {
                    if (form.ShowDialog() == DialogResult.OK) {
                        SelectedFolders = form.SelectedFolders;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType()}, {ex.GetType()}");
            }
        }

        private async Task<long> UploadToStash() {
			return await _client.StashSubmitAsync(new DeviantArtFs.RequestTypes.StashSubmitRequest(
				PostConverter.CreateFilename(_downloaded),
				PostConverter.GetContentType(_downloaded),
				_downloaded.data
			) {
				ArtistComments = txtArtistComments.Text,
				ItemId = _stashItemId,
				IsDirty = false,
				OriginalUrl = _originalUrl,
				Tags = new HashSet<string>(txtTags.Text.Replace("#", "").Replace(",", "").Split(' ').Where(s => s != "")),
				Title = txtTitle.Text
			});
        }

        private async void btnUpload_Click(object sender, EventArgs e) {
            try {
				this.Enabled = false;

                long itemId = await UploadToStash();

                StringBuilder url = new StringBuilder();
                while (itemId > 0) {
                    url.Insert(0, "0123456789abcdefghijklmnopqrstuvwxyz"[(int)(itemId % 36)]);
                    itemId /= 36;
                }
                url.Insert(0, "https://sta.sh/0");
				this.Uploaded?.Invoke(url.ToString());
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, $"{GetType()} {ex.GetType()}");
            }

			this.Enabled = true;
        }

        private async void btnPublish_Click(object sender, EventArgs e) {
            if (chkAgree.Checked == false) {
                MessageBox.Show("Before submitting to DeviantArt, you must agree to the Submission Policy and the Terms of Service.");
                return;
            }

            try {
				this.Enabled = false;
				throw new NotImplementedException();
    //            var classifications = new HashSet<PublishRequest.ClassificationOfMature>();
    //            if (chkNudity.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Nudity);
    //            if (chkSexual.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Sexual);
    //            if (chkGore.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Gore);
    //            if (chkLanguage.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Language);
    //            if (chkIdeology.Checked) classifications.Add(PublishRequest.ClassificationOfMature.Ideology);

    //            var item = await UploadToStash();

    //            var sharingStr = ddlSharing.SelectedItem?.ToString();
    //            var sharing = sharingStr == "Show share buttons" ? PublishRequest.SharingOption.Allow
    //                    : sharingStr == "Hide share buttons" ? PublishRequest.SharingOption.HideShareButtons
    //                    : sharingStr == "Hide & require login to view" ? PublishRequest.SharingOption.HideAndMembersOnly
    //                    : throw new Exception("Unrecognized ddlSharing.SelectedItem");

    //            var r2 = await new PublishRequest(
    //                !radNone.Checked,
    //                chkAgree.Checked,
    //                chkAgree.Checked,
    //                item.ItemId
    //            ) {
    //                MatureLevel = radStrict.Checked
    //                    ? PublishRequest.LevelOFMature.Strict
    //                    : PublishRequest.LevelOFMature.Moderate,
    //                MatureClassification = classifications,
    //                CatPath = SelectedCategory?.CategoryPath,
    //                AllowComments = chkAllowComments.Checked,
    //                RequestCritique = chkRequestCritique.Checked,
    //                Sharing = sharing,
    //                LicenseCreativeCommons = ddlLicense.SelectedText.Contains("CC-"),
    //                LicenseComercial = !ddlLicense.SelectedText.Contains("-NC"),
    //                LicenseModify = ddlLicense.SelectedText.Contains("-ND") ? PublishRequest.LicenseModifyOption.No
    //                    : ddlLicense.SelectedText.Contains("-SA") ? PublishRequest.LicenseModifyOption.Share
    //                    : PublishRequest.LicenseModifyOption.Yes,
    //                GalleryIds = new HashSet<string>(SelectedFolders == null
    //                    ? Enumerable.Empty<string>()
    //                    : SelectedFolders.Select(f => f.FolderId)),
    //                AllowFreeDownload = chkAllowFreeDownload.Checked
    //            }.ExecuteAsync();
    //            if (r2.IsError) {
    //                throw new Exception("Posted to sta.sh but could not post to DeviantArt: " + r2.ErrorText + Environment.NewLine);
    //            }
    //            if (!string.IsNullOrEmpty(r2.Result.Error)) {
    //                throw new Exception("Posted to sta.sh but could not post to DeviantArt: " + r2.Result.ErrorDescription);
    //            }

				//this.Uploaded?.Invoke(r2.Result.Url);
			} catch (Exception ex) {
                MessageBox.Show(this, ex.Message, $"{GetType()} {ex.GetType()}");
            }

			this.Enabled = true;
        }

        //private void ShowHTMLDialog(DeviantartApi.Requests.Request<DeviantartApi.Objects.Information> request, string fallbackUrl) {
        //    using (var form = new Form()) {
        //        form.Width = 400;
        //        form.Height = 600;
        //        var browser = new WebBrowser {
        //            Dock = DockStyle.Fill
        //        };
        //        form.Controls.Add(browser);
        //        form.Load += async (x, y) => {
        //            browser.Navigate("about:blank");
        //            try {
        //                var result = await request.ExecuteAsync();
        //                if (result.IsError) {
        //                    throw new Exception(result.ErrorText);
        //                }
        //                if (!string.IsNullOrEmpty(result.Result.Error)) {
        //                    throw new Exception(result.Result.ErrorDescription);
        //                }
        //                browser.Document.Write(result.Result.Text);
        //            } catch (Exception) {
        //                browser.Navigate(fallbackUrl);
        //            }
        //        };
        //        form.ShowDialog(this);
        //    }
        //}

        private void lnkSubmissionPolicy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			throw new NotImplementedException();
			//ShowHTMLDialog(new DeviantartApi.Requests.Data.SubmissionRequest(), "https://about.deviantart.com/policy/submission/");
        }

        private void lnkTermsOfService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			throw new NotImplementedException();
			//ShowHTMLDialog(new DeviantartApi.Requests.Data.TermsOfServiceRequest(), "https://about.deviantart.com/policy/service/");
		}
	}
}
