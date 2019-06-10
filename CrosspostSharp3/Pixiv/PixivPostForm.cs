using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixivUploader;
using SourceWrappers;

namespace CrosspostSharp3 {
	public partial class PixivPostForm : Form, INewSubmission {
		private readonly Settings.PixivUploadSettings _settings;

		private readonly MemoryStream _ms;

		public PixivPostForm(Settings.PixivUploadSettings settings, TextPost post, IDownloadedData downloaded = null) {
			_settings = settings;

			_ms = new MemoryStream(downloaded.Data, false);

			InitializeComponent();

			foreach (var control in new[] { radAllAges, radR18, radR18G }) {
				if (control is RadioButton r) {
					r.CheckedChanged += (o, e) => {
						groupBox2.Enabled = radAllAges.Checked;
						groupBox3.Enabled = !radAllAges.Checked;
					};
				}
			}

			txtTitle.Text = post.Title;
			txtDescription.Text = HtmlConversion.ConvertHtmlToText(post.HTMLDescription);
			txtTags.Text = string.Join(" ", post.Tags.Select(s => s.Replace(" ", "_")));

			if (post.Adult) {
				radR18.Checked = true;
				radSexualImplicit.Checked = true;
			} else if (post.Mature) {
				radAllAges.Checked = true;
				radSexualImplicit.Checked = true;
			} else {
				radAllAges.Checked = true;
				radSexualNone.Checked = true;
			}
		}

		Stream INewSubmission.Data => _ms;
		string INewSubmission.Title => txtTitle.Text;
		string INewSubmission.Description => txtDescription.Text;
		IEnumerable<string> INewSubmission.Tag => txtTags.Text
			.Split(' ')
			.Select(s => s.Trim().Replace("#", ""))
			.Where(s => s != "");
		ViewingRestriction INewSubmission.ViewingRestriction =>
			radAllAges.Checked ? ViewingRestriction.AllAges
			: radR18.Checked ? ViewingRestriction.R18
			: radR18G.Checked ? ViewingRestriction.R18G
			: throw new Exception("You must choose a viewing restriction.");
		SexualContent INewSubmission.ImplicitSexualContent =>
			radSexualImplicit.Checked ? SexualContent.Implicit
			: radSexualNone.Checked ? SexualContent.None
			: throw new Exception("You must declare whether the work has implicit sexual content.");
		bool INewSubmission.Minors => chkMinors.Checked;
		bool INewSubmission.Furry => chkFurry.Checked;
		bool INewSubmission.BL => chkBl.Checked;
		bool INewSubmission.GL => chkGl.Checked;
		PrivacySettings INewSubmission.PrivacySettings =>
			radPublic.Checked ? PrivacySettings.Public
			: radMyPixivOnly.Checked ? PrivacySettings.MyPixivOnly
			: radPrivate.Checked ? PrivacySettings.Private
			: throw new Exception("You must choose a privacy setting.");
		bool INewSubmission.OriginalWork => chkOriginalWork.Checked;

		private async void BtnPost_Click(object sender, EventArgs e) {
			try {
				btnPost.Enabled = false;
				await Uploader.UploadIllustrationAsync(_settings, this);
				Close();
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			_ms.Dispose();
			base.Dispose(disposing);
		}
	}
}
