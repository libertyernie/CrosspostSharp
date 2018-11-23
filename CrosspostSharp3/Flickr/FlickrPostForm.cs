using FlickrNet;
using FurryNetworkLib;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class FlickrPostForm : Form {
		private readonly Flickr _client;
		private readonly SavedPhotoPost _artworkData;

		public FlickrPostForm(Settings.FlickrSettings s, SavedPhotoPost d) {
			InitializeComponent();
			_client = s.CreateClient();
			_artworkData = d;
			lblUsername1.Text = s.username;

			txtTitle.Text = d.title;
			txtDescription.Text = Regex.Replace(d.description ?? "", @"<br ?\/?>\r?\n?", Environment.NewLine);
			txtTags.Text = string.Join(" ", d.tags);

			if (_artworkData.adult) {
				radFlickrRestricted.Checked = true;
			} else if (_artworkData.mature) {
				radFlickrModerate.Checked = true;
			} else {
				radFlickrSafe.Checked = true;
			}
		}

		private async void Form_Shown(object sender, EventArgs e) {
			try {
				string avatar = await new FlickrSourceWrapper(_client).GetUserIconAsync();
				if (avatar != null) {
					var req = WebRequestFactory.Create(avatar);
					using (var resp = await req.GetResponseAsync())
					using (var stream = resp.GetResponseStream())
					using (var ms = new MemoryStream()) {
						await stream.CopyToAsync(ms);
						ms.Position = 0;
						picUserIcon.Image = Image.FromStream(ms);
					}
				}

				var t = new TaskCompletionSource<LicenseCollection>();
				_client.PhotosLicensesGetInfoAsync(result => {
					if (result.HasError) {
						t.SetException(result.Error);
					} else {
						t.SetResult(result.Result);
					}
				});
				var licenses = await t.Task;

				ddlFlickrLicense.Items.Clear();
				ddlFlickrLicense.DisplayMember = nameof(License.LicenseName);
				ddlFlickrLicense.Items.AddRange(licenses.Where(l => (int)l.LicenseId != 7).ToArray());
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			try {
				var contentType = radFlickrPhoto.Checked ? ContentType.Photo
						: radFlickrScreenshot.Checked ? ContentType.Screenshot
						: radFlickrOther.Checked ? ContentType.Other
						: ContentType.None;
				var safetyLevel = radFlickrSafe.Checked ? SafetyLevel.Safe
					: radFlickrModerate.Checked ? SafetyLevel.Moderate
					: radFlickrRestricted.Checked ? SafetyLevel.Restricted
					: SafetyLevel.None;
				using (var ms = new MemoryStream(_artworkData.data, false)) {
					var t1 = new TaskCompletionSource<string>();
					
					_client.UploadPictureAsync(
						ms,
						PostConverter.CreateFilename(_artworkData),
						txtTitle.Text,
						txtDescription.Text,
						txtTags.Text.Replace("#", ""),
						chkFlickrPublic.Checked,
						chkFlickrFamily.Checked,
						chkFlickrFriend.Checked,
						contentType,
						safetyLevel,
						chkFlickrHidden.Checked ? HiddenFromSearch.Hidden : HiddenFromSearch.Visible,
						result => {
							if (result.HasError) {
								t1.SetException(result.Error);
							} else {
								t1.SetResult(result.Result);
							}
						});

					string photoId = await t1.Task;
					
					var license = ddlFlickrLicense.SelectedItem as License;
					if (license != null) {
						var t2 = new TaskCompletionSource<NoResponse>();
						_client.PhotosLicensesSetLicenseAsync(photoId, license.LicenseId, result => {
							if (result.HasError) {
								t2.SetException(result.Error);
							} else {
								t2.SetResult(result.Result);
							}
						});
						await t2.Task;
					}
				}

				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
