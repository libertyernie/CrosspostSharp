﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LWeasyl;
using System.Net.Mail;

namespace WeasylSync {
	public partial class Form1 : Form {
		public static string USERNAME = ConfigurationManager.AppSettings["weasyl-username"];

		private WeasylThumbnail[] thumbnails;

		private byte[] currentImage;
		public byte[] CurrentImage {
			get {
				return currentImage;
			}
			set {
				currentImage = value;
				if (value == null) {
					mainPictureBox.Image = null;
				} else {
					Image image = null;
					try {
						image = Bitmap.FromStream(new MemoryStream(value));
						mainPictureBox.Image = image;
					} catch (ArgumentException) {
						MessageBox.Show("This submission is not an image file.");
						mainPictureBox.Image = null;
					}
				}
			}
		}

		private SubmissionDetail details;
		public SubmissionDetail Details {
			get {
				return details;
			}
			set {
				details = value;
				if (value != null) {
					txtTitle.Text = value.title;
					txtDescription.Text = value.description;
					lblLink.Text = value.link;
					txtTags.Text = string.Join(" ", value.tags.Select(s => "#" + s));
					pickDate.Value = pickTime.Value = value.posted_at;
				}
			}
		}

		private WebClient client;

		private int? backid, nextid;
		private float originalFontSize;

		public Form1() {
			InitializeComponent();
			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3, thumbnail4 };
			client = new WebClient();
			originalFontSize = txtTitle.Font.SizeInPoints;

			backid = nextid = null;
			Task<Gallery> t = new Task<Gallery>(() => {
				return APIInterface.UserGallery(USERNAME, count: this.thumbnails.Length);
			});
			t.Start();
			t.GetAwaiter().OnCompleted(new Action(() => {
				PopulateThumbnails(t.Result);
			}));
		}

		public void PopulateThumbnails(Gallery gallery) {
			for (int i = 0; i < this.thumbnails.Length; i++) {
				if (i < gallery.submissions.Length) {
					this.thumbnails[i].Submission = gallery.submissions[i];
				} else {
					this.thumbnails[i].Submission = null;
				}
			}
			this.backid = gallery.backid;
			btnUp.Enabled = (backid != null);
			this.nextid = gallery.nextid;
			btnDown.Enabled = (nextid != null);
		}

		private void btnUp_Click(object sender, EventArgs e) {
			PopulateThumbnails(APIInterface.UserGallery(USERNAME, count: this.thumbnails.Length, backid: this.backid));
		}

		private void btnDown_Click(object sender, EventArgs e) {
			PopulateThumbnails(APIInterface.UserGallery(USERNAME, count: this.thumbnails.Length, nextid: this.nextid));
		}

		private void chkTitleBold_CheckedChanged(object sender, EventArgs e) {
			txtTitleSize_TextChanged(sender, e);
		}

		private void txtTitleSize_TextChanged(object sender, EventArgs e) {
			txtTitle.Font = new Font(txtTitle.Font.FontFamily, txtTitle.Font.Size, chkTitleBold.Checked ? FontStyle.Bold : FontStyle.Regular);
		}

		private void btnEmail_Click(object sender, EventArgs e) {
			StringBuilder sb = new StringBuilder();
			if (chkTitle.Checked) {
				List<string> styles = new List<string>();
				if (chkTitleBold.Checked) sb.Append("**");
				sb.Append(txtTitle.Text);
				if (chkTitleBold.Checked) sb.Append("**");
				sb.Append("\n\n");
			}
			if (chkDescription.Checked) {
				sb.Append(txtDescription.Text);
				sb.Append("\n\n");
			}
			if (chkLink.Checked) {
				sb.Append("[" + txtLink.Text + "](lblLink.Text)");
				sb.Append("\n\n");
			}
			if (chkWeasylTag.Checked) {
				sb.Append("#weasyl ");
			}
			if (chkTags.Checked) {
				sb.Append(txtTags.Text);
			}
			System.IO.File.WriteAllText("C:/Users/Owner/Desktop/dump.html", sb.ToString());
			System.Diagnostics.Process.Start("C:/Users/Owner/Desktop/dump.html");
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Enabled = pickTime.Enabled = !chkNow.Checked;
		}
	}
}