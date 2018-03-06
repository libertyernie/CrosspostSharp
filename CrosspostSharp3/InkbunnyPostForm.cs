using ArtSourceWrapper;
using InkbunnyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class InkbunnyPostForm : Form {
		private readonly InkbunnyClient _client;
		private readonly ArtworkData _artworkData;

		public InkbunnyPostForm(Settings.InkbunnySettings s, ArtworkData d) {
			InitializeComponent();
			_client = new InkbunnyClient(s.sid, s.userId);
			_artworkData = d;
			lblUsername1.Text = s.username;

			txtTitle.Text = d.title;
			txtDescription.Text = HtmlConversion.ConvertHtmlToText(d.description);
			txtTags.Text = string.Join(" ", d.tags);
		}

		private async void Form_Shown(object sender, EventArgs e) {
			try {
				var wrapper = new InkbunnyWrapper(_client);
				wrapper.BatchSize = 1;
				lblUsername1.Text = await wrapper.WhoamiAsync();

				var req = WebRequestFactory.Create(await wrapper.GetUserIconAsync(picUserIcon.Width));
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					picUserIcon.Image = Image.FromStream(ms);
				}
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			try {
				var rating = new List<InkbunnyRatingTag>();
				//if (chkInbunnyTag2.Checked) rating.Add(InkbunnyRatingTag.Nudity);
				//if (chkInbunnyTag3.Checked) rating.Add(InkbunnyRatingTag.Violence);
				//if (chkInbunnyTag4.Checked) rating.Add(InkbunnyRatingTag.SexualThemes);
				//if (chkInbunnyTag5.Checked) rating.Add(InkbunnyRatingTag.StrongViolence);

				long submission_id = await _client.UploadAsync(files: new byte[][] {
					_artworkData.data
				});

				var o = await _client.EditSubmissionAsync(
					submission_id: submission_id,
					title: txtTitle.Text,
					desc: txtDescription.Text,
					convert_html_entities: true,
					type: InkbunnySubmissionType.Picture,
					scraps: chkInkbunnyScraps.Checked,
					isPublic: chkInkbunnyPublic.Checked,
					notifyWatchersWhenPublic: chkInkbunnyNotifyWatchers.Checked,
					keywords: txtTags.Text.Replace("#", "").Split(' ').Where(s => s != ""),
					tag: rating
				);
				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void chkInkbunnyPublic_CheckedChanged(object sender, EventArgs e) {
			chkInkbunnyNotifyWatchers.Enabled = chkInkbunnyPublic.Checked;
		}
	}
}
