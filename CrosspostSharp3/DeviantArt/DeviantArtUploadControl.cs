using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using DeviantArtFs.ParameterTypes;
using DeviantArtFs.ResponseTypes;
using DeviantArtFs.SubmissionTypes;
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

		private IEnumerable<GalleryFolder> _selectedFolders;
		public IEnumerable<GalleryFolder> SelectedFolders {
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

			ddlLicense.Items.Clear();
			ddlLicense.Items.AddRange(DeviantArtLicense.ListAll().ToArray());

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
			MessageBox.Show(this, "This feature is no longer supported.");
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
				var resp = await DeviantArtFs.Api.Stash.AsyncSubmit(
					_token,
					SubmissionDestination.Default,
					new SubmissionParameters(
						SubmissionTitle.NewSubmissionTitle(txtTitle.Text),
						ArtistComments.NewArtistComments(txtArtistComments.Text),
						TagList.Create(txtTags.Text.Replace("#", "").Replace(",", "").Split(' ').Where(s => s != "")),
						OriginalUrl.NoOriginalUrl,
						is_dirty: false),
					_downloaded).StartAsTask();
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

				var classifications = new List<MatureClassification>();
				if (chkNudity.Checked) classifications.Add(MatureClassification.Nudity);
				if (chkSexual.Checked) classifications.Add(MatureClassification.Sexual);
				if (chkGore.Checked) classifications.Add(MatureClassification.Gore);
				if (chkLanguage.Checked) classifications.Add(MatureClassification.Language);
				if (chkIdeology.Checked) classifications.Add(MatureClassification.Ideology);

				var sharingStr = ddlSharing.SelectedItem?.ToString();
				var sharing = sharingStr == "Show share buttons" ? Sharing.AllowSharing
					: sharingStr == "Hide share buttons" ? Sharing.HideShareButtons
					: sharingStr == "Hide & require login to view" ? Sharing.HideShareButtonsAndMembersOnly
					: throw new Exception("Unrecognized ddlSharing.SelectedItem");

				var resp = await DeviantArtFs.Api.Stash.AsyncPublish(
					_token,
					new PublishParameters(
						maturity: radNone.Checked
							? Maturity.NotMature
							: Maturity.NewMature(
								radStrict.Checked ? MatureLevel.MatureStrict : MatureLevel.MatureModerate,
								MatureClassificationSet.Create(classifications)),
						submissionPolicyAgreement: chkAgree.Checked,
						termsOfServiceAgreement: chkAgree.Checked,
						featured: PublishParameters.Default.featured,
						allowComments: chkAllowComments.Checked,
						requestCritique: chkRequestCritique.Checked,
						displayResolution: PublishParameters.Default.displayResolution,
						sharing: sharing,
						license: (ddlLicense.SelectedItem as DeviantArtLicense)?.License ?? throw new Exception("No license selected"),
						destinations: GallerySet.Create(SelectedFolders.Select(f => f.folderid)),
						allowFreeDownload: chkAllowFreeDownload.Checked,
						addWatermark: PublishParameters.Default.addWatermark),
					StashItem.NewStashItem(item)).StartAsTask();

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
			var resp = await DeviantArtFs.Api.Data.AsyncGetSubmissionPolicy(_token).StartAsTask();
			ShowHTMLDialog(resp.text);
		}

		private async void lnkTermsOfService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			var resp = await DeviantArtFs.Api.Data.AsyncGetTermsOfService(_token).StartAsTask();
			ShowHTMLDialog(resp.text);
		}
	}
}
