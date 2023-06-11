using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CrosspostSharp3.Inkbunny {
	public partial class InkbunnyPostForm : Form {
		private readonly InkbunnyClient _client;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public InkbunnyPostForm(Settings.InkbunnySettings s, TextPost post, IDownloadedData downloaded) {
			InitializeComponent();
			_client = new InkbunnyClient(s.sid, s.userId);
			_post = post;
			_downloaded = downloaded;
			lblUsername1.Text = s.username;

			txtTitle.Text = post.Title;
			txtDescription.Text = HtmlConversion.ConvertHtmlToText(post.HTMLDescription);
			txtTags.Text = string.Join(" ", post.Tags);
		}

		private async void Form_Shown(object sender, EventArgs e) {
			try {
				var result = await _client.SearchAsync(
					new InkbunnySearchParameters { UserId = _client.UserId },
					submissions_per_page: 1,
					get_rid: false);
				if (result.submissions.FirstOrDefault() is InkbunnySearchSubmission ss) {
					var submission = await _client.GetSubmissionAsync(ss.submission_id);

					lblUsername1.Text = submission.username;

					var req = WebRequest.Create(submission.user_icon_url_small);
					using (var resp = await req.GetResponseAsync())
					using (var stream = resp.GetResponseStream())
					using (var ms = new MemoryStream()) {
						await stream.CopyToAsync(ms);
						ms.Position = 0;
						picUserIcon.Image = Image.FromStream(ms);
					}
				}
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			try {
				var rating = new List<InkbunnyRatingTag>();
				if (chkInkbunnyTag2.Checked) rating.Add(InkbunnyRatingTag.Nudity);
				if (chkInkbunnyTag3.Checked) rating.Add(InkbunnyRatingTag.Violence);
				if (chkInkbunnyTag4.Checked) rating.Add(InkbunnyRatingTag.SexualThemes);
				if (chkInkbunnyTag5.Checked) rating.Add(InkbunnyRatingTag.StrongViolence);

				long submission_id = await _client.UploadAsync(files: new byte[][] {
					_downloaded.Data
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
