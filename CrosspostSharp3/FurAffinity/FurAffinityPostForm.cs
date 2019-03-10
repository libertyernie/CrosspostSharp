using FurAffinityFs;
using SourceWrappers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using FAModels = FurAffinityFs.Models;
using FAReq = FurAffinityFs.Requests;

namespace CrosspostSharp3 {
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

			foreach (var x in Enum.GetValues(typeof(FAModels.Category))) {
				ddlCategory.Items.Add((FAModels.Category)x);
			}
			foreach (var x in Enum.GetValues(typeof(FAModels.Type))) {
				ddlTheme.Items.Add((FAModels.Type)x);
			}
			foreach (var x in Enum.GetValues(typeof(FAModels.Species))) {
				ddlSpecies.Items.Add((FAModels.Species)x);
			}
			foreach (var x in Enum.GetValues(typeof(FAModels.Gender))) {
				ddlGender.Items.Add((FAModels.Gender)x);
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
				string username = await FAReq.Whoami.ExecuteAsync(_credentials);
				if (username == null) {
					lblUsername1.Text = "Not logged in";
				} else {
					lblUsername1.Text = username;

					Uri avatar = await FAReq.GetAvatar.ExecuteAsync(_credentials, username);
					if (avatar != null) {
						var req = WebRequestFactory.Create(avatar.AbsoluteUri);
						using (var resp = await req.GetResponseAsync())
						using (var stream = resp.GetResponseStream())
						using (var ms = new MemoryStream()) {
							await stream.CopyToAsync(ms);
							ms.Position = 0;
							picUserIcon.Image = Image.FromStream(ms);
						}
					}
				}
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

				await FAReq.SubmitPost.ExecuteAsync(_credentials, new FAModels.Submission(
					data: data,
					contentType: contentType,
					title: txtTitle.Text,
					message: txtDescription.Text,
					keywords: txtTags.Text.Split(' ').Select(s => s.Trim()).Where(s => s != ""),
					cat: (FAModels.Category)ddlCategory.SelectedItem,
					scrap: chkScraps.Checked,
					atype: (FAModels.Type)ddlTheme.SelectedItem,
					species: (FAModels.Species)ddlSpecies.SelectedItem,
					gender: (FAModels.Gender)ddlGender.SelectedItem,
					rating: radRating0.Checked ? FAModels.Rating.General
						: radRating1.Checked ? FAModels.Rating.Mature
						: radRating2.Checked ? FAModels.Rating.Adult
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
