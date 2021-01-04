using ArtworkSourceSpecification;
using FurAffinityFs;
using SourceWrappers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3.FurAffinity {
	public partial class FurAffinityPostForm : Form {
		private readonly IFurAffinityCredentials _credentials;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public FurAffinityPostForm(IFurAffinityCredentials s, TextPost post, IDownloadedData downloaded) {
			InitializeComponent();
			_credentials = s;
			_post = post;
			_downloaded = downloaded;

			txtTitle.Text = post.Title;
			txtDescription.Enabled = false;
			txtTags.Text = string.Join(" ", post.Tags.Where(t => t.Length >= 3));

			if (post.Adult) {
				radRating2.Checked = true;
			} else if (post.Mature) {
				radRating1.Checked = true;
			} else {
				radRating0.Checked = true;
			}

			foreach (var x in Enum.GetValues(typeof(FurAffinityCategory))) {
				ddlCategory.Items.Add((FurAffinityCategory)x);
			}
			foreach (var x in Enum.GetValues(typeof(FurAffinityType))) {
				ddlTheme.Items.Add((FurAffinityType)x);
			}
			foreach (var x in Enum.GetValues(typeof(FurAffinitySpecies))) {
				ddlSpecies.Items.Add((FurAffinitySpecies)x);
			}
			foreach (var x in Enum.GetValues(typeof(FurAffinityGender))) {
				ddlGender.Items.Add((FurAffinityGender)x);
			}
		}

		private static bool HasAlpha(Image image) {
			return image.PixelFormat.HasFlag(PixelFormat.Alpha);
		}

		private void Form_Shown(object sender, EventArgs e) {
			PopulateDescription();
			PopulateIcon();

			using (var ms = new MemoryStream(_downloaded.Data, false))
			using (var image = Image.FromStream(ms)) {
				chkRemoveTransparency.Enabled = HasAlpha(image);
			}
		}

		private void PopulateDescription() {
			try {
				txtDescription.Text = HtmlConversion.ConvertHtmlToText(_post.HTMLDescription);
			} catch (Exception) { }
			txtDescription.Enabled = true;
		}

		private async void PopulateIcon() {
			try {
				var user = await FAExportArtworkSource.GetCurrentProfileAsync($"b={_credentials.B}; a={_credentials.A}", false);
				lblUsername1.Text = user.name;
				picUserIcon.ImageLocation = user.avatar;
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			try {
				string contentType;
				byte[] data = _downloaded.Data;
				using (var ms = new MemoryStream(_downloaded.Data, false))
				using (var image = Image.FromStream(ms)) {
					contentType = image.RawFormat.Equals(ImageFormat.Png) ? "image/png"
						: image.RawFormat.Equals(ImageFormat.Jpeg) ? "image/jpeg"
						: image.RawFormat.Equals(ImageFormat.Gif) ? "image/gif"
						: throw new ApplicationException("Only JPEG, GIF, and PNG images are supported.");

					if (chkRemoveTransparency.Checked) {
						using (var newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb)) {
							using (var g = Graphics.FromImage(newImage)) {
								g.FillRectangle(new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
								g.DrawImage(image, 0, 0, image.Width, image.Height);
							}

							using (var msout = new MemoryStream()) {
								newImage.Save(msout, ImageFormat.Png);
								data = msout.ToArray();
								contentType = "image/png";
							}
						}
					}
				}

				await FurAffinitySubmission.PostArtworkAsync(
					_credentials,
					new FurAffinityFile(data, contentType),
					new FurAffinitySubmission.ArtworkMetadata(
						title: txtTitle.Text,
						message: txtDescription.Text,
						keywords: txtTags.Text.Split(' ').Select(s => s.Trim()).Where(s => s != ""),
						cat: (FurAffinityCategory)ddlCategory.SelectedItem,
						scrap: chkScraps.Checked,
						atype: (FurAffinityType)ddlTheme.SelectedItem,
						species: (FurAffinitySpecies)ddlSpecies.SelectedItem,
						gender: (FurAffinityGender)ddlGender.SelectedItem,
						rating: radRating0.Checked ? FurAffinityRating.General
							: radRating1.Checked ? FurAffinityRating.Mature
							: radRating2.Checked ? FurAffinityRating.Adult
							: throw new ApplicationException("Must select a rating"),
						lock_comments: chkLockComments.Checked
					));

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
