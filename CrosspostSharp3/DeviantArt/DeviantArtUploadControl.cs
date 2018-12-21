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

		private readonly IDeviantArtAccessToken _token;

		public DeviantArtUploadControl(IDeviantArtAccessToken token) {
			InitializeComponent();
			_token = token;

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
			using (var f = new DeviantArtCategoryBrowser(_token)) {
				f.InitialCategory = SelectedCategory;
				if (f.ShowDialog() == DialogResult.OK) {
					SelectedCategory = f.SelectedCategory;
				}
			}
		}

		private void btnGalleryFolders_Click(object sender, EventArgs e) {
			try {
				using (var form = new DeviantArtFolderSelectionForm(_token)) {
					if (form.ShowDialog() == DialogResult.OK) {
						SelectedFolders = form.SelectedFolders;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType()}, {ex.GetType()}");
			}
		}

		private async Task<long> UploadToStash() {
			return await DeviantArtFs.Stash.Submit.ExecuteAsync(_token, new DeviantArtFs.Stash.SubmitRequest(
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

				var item = await UploadToStash();

				var classifications = DeviantArtFs.Stash.MatureClassification.None;
				if (chkNudity.Checked) classifications |= DeviantArtFs.Stash.MatureClassification.Nudity;
				if (chkSexual.Checked) classifications |= DeviantArtFs.Stash.MatureClassification.Sexual;
				if (chkGore.Checked) classifications |= DeviantArtFs.Stash.MatureClassification.Gore;
				if (chkLanguage.Checked) classifications |= DeviantArtFs.Stash.MatureClassification.Language;
				if (chkIdeology.Checked) classifications |= DeviantArtFs.Stash.MatureClassification.Ideology;

				var sharingStr = ddlSharing.SelectedItem?.ToString();
				var sharing = sharingStr == "Show share buttons" ? DeviantArtFs.Stash.Sharing.Allow
						: sharingStr == "Hide share buttons" ? DeviantArtFs.Stash.Sharing.HideShareButtons
						: sharingStr == "Hide & require login to view" ? DeviantArtFs.Stash.Sharing.HideAndMembersOnly
						: throw new Exception("Unrecognized ddlSharing.SelectedItem");

				var req = new DeviantArtFs.Stash.PublishRequest(item) {
					IsMature = !radNone.Checked,
					AgreeSubmission = chkAgree.Checked,
					AgreeTos = chkAgree.Checked,
					MatureLevel = radStrict.Checked
						? DeviantArtFs.Stash.MatureLevel.Strict
						: DeviantArtFs.Stash.MatureLevel.Moderate,
					MatureClassification = classifications,
					Catpath = SelectedCategory?.CategoryPath,
					AllowComments = chkAllowComments.Checked,
					RequestCritique = chkRequestCritique.Checked,
					Sharing = sharing,
					LicenseOptions = new DeviantArtFs.Stash.LicenseOptions {
						CreativeCommons = ddlLicense.SelectedItem.ToString().Contains("CC-"),
						Commercial = !ddlLicense.SelectedItem.ToString().Contains("-NC"),
						Modify = ddlLicense.SelectedItem.ToString().Contains("-ND") ? DeviantArtFs.Stash.LicenseModifyOption.No
							: ddlLicense.SelectedItem.ToString().Contains("-SA") ? DeviantArtFs.Stash.LicenseModifyOption.ShareAlike
							: DeviantArtFs.Stash.LicenseModifyOption.Yes,
					},
					GalleryIds = SelectedFolders == null
						? Enumerable.Empty<Guid>()
						: SelectedFolders.Select(f => f.FolderId),
					AllowFreeDownload = chkAllowFreeDownload.Checked
				};

				var resp = await DeviantArtFs.Stash.Publish.ExecuteAsync(_token, req);

				Uploaded?.Invoke(resp.Url);
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, $"{GetType()} {ex.GetType()}");
			}

			this.Enabled = true;
		}

		private void ShowHTMLDialog(string html) {
			using (var form = new Form()) {
				form.Width = 400;
				form.Height = 600;
				var browser = new WebBrowser {
					Dock = DockStyle.Fill
				};
				form.Controls.Add(browser);
				form.Load += (x, y) => {
					browser.Navigate("about:blank");
					browser.Document.Write(html);
				};
				form.ShowDialog(this);
			}
		}

		private async void lnkSubmissionPolicy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			string html = await DeviantArtFs.Data.Submission.ExecuteAsync(_token);
			ShowHTMLDialog(html);
		}

		private async void lnkTermsOfService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			string html = await DeviantArtFs.Data.Tos.ExecuteAsync(_token);
			ShowHTMLDialog(html);
		}
	}
}
