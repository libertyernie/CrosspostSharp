using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fsfs = FurAffinityFs.FurAffinity;

namespace CrosspostSharp3.FurAffinity {
	public partial class FurAffinityPostForm : Form {
		private readonly Fsfs.ICredentials _credentials;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public FurAffinityPostForm(Fsfs.ICredentials s, TextPost post, IDownloadedData downloaded) {
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

			foreach (var x in Enum.GetValues(typeof(Fsfs.Category))) {
				ddlCategory.Items.Add((Fsfs.Category)x);
			}
			foreach (var x in Enum.GetValues(typeof(Fsfs.Type))) {
				ddlTheme.Items.Add((Fsfs.Type)x);
			}
			foreach (var x in Enum.GetValues(typeof(Fsfs.Gender))) {
				ddlGender.Items.Add((Fsfs.Gender)x);
			}
		}

		private static bool HasAlpha(Image image) {
			return image.PixelFormat.HasFlag(PixelFormat.Alpha);
		}

		private async void Form_Shown(object sender, EventArgs e) {
			PopulateDescription();
			await PopulateIcon();

			using (var ms = new MemoryStream(_downloaded.Data, false))
			using (var image = Image.FromStream(ms)) {
				chkRemoveTransparency.Enabled = HasAlpha(image);
			}

			try {
				var species = await Fsfs.ListSpeciesAsync();
				foreach (var x in species) {
					ddlSpecies.Items.Add(x);
				}
			} catch (Exception ex) {
				Console.Error.WriteLine(ex);
			}

			foreach (var galleryFolder in await Fsfs.ListGalleryFoldersAsync(_credentials)) {
				listBox1.Items.Add(galleryFolder);
			}
		}

		private void PopulateDescription() {
			try {
				txtDescription.Text = HtmlConversion.ConvertHtmlToText(_post.HTMLDescription);
			} catch (Exception) { }
			txtDescription.Enabled = true;
		}

		private async Task PopulateIcon() {
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

				IEnumerable<long> folderIds() {
					foreach (var item in listBox1.SelectedItems) {
						if (item is Fsfs.ExistingGalleryFolderInformation f) {
							yield return f.FolderId;
						}
					}
				}

				await Fsfs.PostArtworkAsync(
					_credentials,
					new Fsfs.File(data, contentType),
					new Fsfs.ArtworkMetadata(
						title: txtTitle.Text,
						message: txtDescription.Text,
						keywords: Fsfs.Keywords(txtTags.Text.Split(' ').Select(s => s.Trim()).Where(s => s != "").ToArray()),
						cat: (Fsfs.Category)ddlCategory.SelectedItem,
						scrap: chkScraps.Checked,
						atype: (Fsfs.Type)ddlTheme.SelectedItem,
						species: ddlSpecies.SelectedItem is Fsfs.SpeciesInformation s
							? s.Species
							: Fsfs.Species.Unspecified,
						gender: (Fsfs.Gender)ddlGender.SelectedItem,
						rating: radRating0.Checked ? Fsfs.Rating.General
							: radRating1.Checked ? Fsfs.Rating.Mature
							: radRating2.Checked ? Fsfs.Rating.Adult
							: throw new ApplicationException("Must select a rating"),
						lock_comments: chkLockComments.Checked,
						folder_ids: Fsfs.FolderIds(folderIds().ToArray()),
						create_folder_name: Fsfs.NoNewFolder
					));

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
