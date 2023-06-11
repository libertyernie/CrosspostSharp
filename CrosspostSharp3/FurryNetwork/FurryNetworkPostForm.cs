﻿using CrosspostSharp3.FurryNetwork;
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

namespace CrosspostSharp3.FurryNetwork {
	public partial class FurryNetworkPostForm : Form {
		private readonly FurryNetworkClient _client;
		private readonly string _characterName;
		private readonly TextPost _post;
		private readonly IDownloadedData _downloaded;

		public FurryNetworkPostForm(Settings.FurryNetworkSettings s, TextPost post, IDownloadedData downloaded) {
			InitializeComponent();
			_client = new FurryNetworkClient(s.refreshToken);
			_post = post;
			_downloaded = downloaded;
			_characterName = s.characterName;
			lblUsername1.Text = s.characterName;

			txtTitle.Text = post.Title;
			txtDescription.Enabled = false;
			txtTags.Text = string.Join(" ", post.Tags.Where(t => t.Length >= 3));

			if (post.Adult) {
				radFurryNetworkRating2.Checked = true;
			} else if (post.Mature) {
				radFurryNetworkRating1.Checked = true;
			} else {
				radFurryNetworkRating0.Checked = true;
			}
		}

		private void Form_Shown(object sender, EventArgs e) {
			PopulateDescription();
			PopulateIcon();
		}

		private void PopulateDescription() {
			try {
				txtDescription.Text = HtmlConversion.ConvertHtmlToText(_post.HTMLDescription);
			} catch (Exception) { }
			txtDescription.Enabled = true;
		}


		private async void PopulateIcon() {
			try {
				var character = await _client.GetCharacterAsync(_characterName);
				string avatar = character.Avatars.Tiny ?? character.Avatars.GetLargest();
				if (avatar != null) {
					var req = WebRequest.Create(avatar);
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
				var user = await _client.GetUserAsync();
				var artwork = await _client.UploadArtwork(
					_characterName,
					_downloaded.Data,
					_downloaded.ContentType,
					_downloaded.Filename);
				await _client.UpdateArtwork(artwork.Id, new FurryNetworkClient.UpdateArtworkParameters {
					Community_tags_allowed = chkFurryNetworkAllowCommunityTags.Checked,
					Description = txtDescription.Text,
					Publish = true,
					Rating = radFurryNetworkRating0.Checked ? 0
						: radFurryNetworkRating1.Checked ? 1
						: 2,
					Status = radFurryNetworkPublic.Checked ? "public"
						: radFurryNetworkUnlisted.Checked ? "unlisted"
						: "draft",
					Tags = txtTags.Text.Replace("#", " ").Split(' ').Where(s => s != ""),
					Title = txtTitle.Text
				});

				Close();
			} catch (WebException ex) {
				string errors = "";
				try {
					using (var sr = new StreamReader(ex.Response.GetResponseStream())) {
						errors = await sr.ReadToEndAsync();
						if (errors.Length > 200) {
							errors = errors.Substring(0, 199) + "…";
						}
					}
				} catch (Exception) { }
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message + ": " + errors, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (Exception ex) {
				btnPost.Enabled = true;
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
