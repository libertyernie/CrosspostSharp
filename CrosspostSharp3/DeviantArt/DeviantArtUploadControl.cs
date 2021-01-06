using ArtworkSourceSpecification;
using DeviantArtFs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3.DeviantArt {
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

		private IEnumerable<DeviantArtGalleryFolder> _selectedFolders;
		public IEnumerable<DeviantArtGalleryFolder> SelectedFolders {
			get {
				return _selectedFolders;
			}
			set {
				_selectedFolders = value;
				txtGalleryFolders.Text = value == null
					? ""
					: string.Join(", ", value.Select(f => f.name));
			}
		}

		private TextPost _post;
		private IDownloadedData _downloaded;
		private long? _stashItemId;

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

		public void SetSubmission(TextPost post, IDownloadedData downloaded, long? stashItemId) {
			_post = post;
			_downloaded = downloaded;
			_stashItemId = stashItemId;
			
			txtTitle.Text = post.Title ?? "";
			txtArtistComments.Text = post.HTMLDescription ?? "";
			txtTags.Text = string.Join(" ", post.Tags?.Select(s => $"#{s}") ?? Enumerable.Empty<string>());
			if (post.Mature) {
				radStrict.Checked = true;
			} else {
				radNone.Checked = true;
			}
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
			try {
				var resp = await DeviantArtFs.Api.Stash.Submit.ExecuteAsync(_token, new DeviantArtFs.Api.Stash.SubmitRequest(
					_downloaded.Filename,
					_downloaded.ContentType,
					_downloaded.Data
				) {
					ArtistComments = txtArtistComments.Text,
					Itemid = _stashItemId,
					IsDirty = false,
					Tags = new HashSet<string>(txtTags.Text.Replace("#", "").Replace(",", "").Split(' ').Where(s => s != "")),
					Title = txtTitle.Text
				});
				return resp.itemid;
			} catch (DeviantArtException ex) when (ex.Message == "Cannot modify this item, it does not belong to this user." && _stashItemId != null) {
				_stashItemId = null;
				return await UploadToStash();
			}
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

				var classifications = new List<DeviantArtFs.Api.Stash.MatureClassification>();
				if (chkNudity.Checked) classifications.Add(DeviantArtFs.Api.Stash.MatureClassification.Nudity);
				if (chkSexual.Checked) classifications.Add(DeviantArtFs.Api.Stash.MatureClassification.Sexual);
				if (chkGore.Checked) classifications.Add(DeviantArtFs.Api.Stash.MatureClassification.Gore);
				if (chkLanguage.Checked) classifications.Add(DeviantArtFs.Api.Stash.MatureClassification.Language);
				if (chkIdeology.Checked) classifications.Add(DeviantArtFs.Api.Stash.MatureClassification.Ideology);

				var sharingStr = ddlSharing.SelectedItem?.ToString();
				var sharing = sharingStr == "Show share buttons" ? DeviantArtFs.Api.Stash.Sharing.Allow
						: sharingStr == "Hide share buttons" ? DeviantArtFs.Api.Stash.Sharing.HideShareButtons
						: sharingStr == "Hide & require login to view" ? DeviantArtFs.Api.Stash.Sharing.HideAndMembersOnly
						: throw new Exception("Unrecognized ddlSharing.SelectedItem");

				var req = new DeviantArtFs.Api.Stash.PublishRequest(item) {
					IsMature = !radNone.Checked,
					AgreeSubmission = chkAgree.Checked,
					AgreeTos = chkAgree.Checked,
					MatureLevel = radStrict.Checked
						? DeviantArtFs.Api.Stash.MatureLevel.Strict
						: DeviantArtFs.Api.Stash.MatureLevel.Moderate,
					MatureClassification = classifications,
					Catpath = SelectedCategory?.CategoryPath,
					AllowComments = chkAllowComments.Checked,
					RequestCritique = chkRequestCritique.Checked,
					Sharing = sharing,
					LicenseOptions = new DeviantArtFs.Api.Stash.LicenseOptions {
						CreativeCommons = ddlLicense.SelectedItem.ToString().Contains("CC-"),
						Commercial = !ddlLicense.SelectedItem.ToString().Contains("-NC"),
						Modify = ddlLicense.SelectedItem.ToString().Contains("-ND") ? DeviantArtFs.Api.Stash.LicenseModifyOption.No
							: ddlLicense.SelectedItem.ToString().Contains("-SA") ? DeviantArtFs.Api.Stash.LicenseModifyOption.ShareAlike
							: DeviantArtFs.Api.Stash.LicenseModifyOption.Yes,
					},
					Galleryids = SelectedFolders == null
						? Enumerable.Empty<Guid>()
						: SelectedFolders.Select(f => f.folderid),
					AllowFreeDownload = chkAllowFreeDownload.Checked
				};

				var resp = await DeviantArtFs.Api.Stash.Publish.ExecuteAsync(_token, req);

				Uploaded?.Invoke(resp.url);
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
			var resp = await DeviantArtFs.Api.Data.Submission.ExecuteAsync(_token);
			ShowHTMLDialog(resp.text);
		}

		private async void lnkTermsOfService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			var resp = await DeviantArtFs.Api.Data.Tos.ExecuteAsync(_token);
			ShowHTMLDialog(resp.text);
		}
	}
}
