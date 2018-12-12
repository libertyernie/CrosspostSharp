using FurAffinityFs;
using FurryNetworkLib;
using SourceWrappers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class FurAffinityPostForm : Form {
		private readonly FurAffinityClient _client;
		private readonly SavedPhotoPost _artworkData;

		public FurAffinityPostForm(Settings.FurAffinitySettings s, SavedPhotoPost d) {
			InitializeComponent();
			_client = new FurAffinityClient(a: s.a, b: s.b);
			_artworkData = d;

			txtTitle.Text = d.title;
			txtDescription.Enabled = false;
			txtTags.Text = string.Join(" ", d.tags.Where(t => t.Length >= 3));

			if (_artworkData.adult) {
				radRating2.Checked = true;
			} else if (_artworkData.mature) {
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

			using (var ms = new MemoryStream(_artworkData.data, false))
			using (var image = Image.FromStream(ms)) {
				chkRemoveTransparency.Enabled = HasAlpha(image);
			}
		}

		private void PopulateDescription() {
			try {
				txtDescription.Text = HtmlConversion.ConvertHtmlToText(_artworkData.description);
			} catch (Exception) { }
			txtDescription.Enabled = true;
		}

		private async void PopulateIcon() {
			try {
				string username = await _client.WhoamiAsync();
				if (username == null) {
					lblUsername1.Text = "Not logged in";
				} else {
					lblUsername1.Text = username;

					Uri avatar = await _client.GetAvatarUriAsync(username);
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
				byte[] data = _artworkData.data;
				using (var ms = new MemoryStream(_artworkData.data, false))
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

				await _client.SubmitPostAsync(new FurAffinitySubmission(
					data: data,
					contentType: contentType,
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
