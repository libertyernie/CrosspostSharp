using ArtSourceWrapper;
using DeviantArtControls;
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

namespace CrosspostSharp3 {
	public partial class ArtworkForm : Form {
		private byte[] _data;
		private string _originalUrl;

		public ArtworkForm(ISubmissionWrapper wrapper) {
			InitializeComponent();
			Text = wrapper.ImageURL.Split('/').Last();
			txtTitle.Text = wrapper.Title;
			txtDescription.Text = wrapper.HTMLDescription;
			txtTags.Text = string.Join(" ", wrapper.Tags);
			Shown += async (o, e) => {
				try {
					var req = WebRequest.Create(wrapper.ImageURL);
					req.Method = "GET";
					if (req is HttpWebRequest httpreq) {
						if (req.RequestUri.Host.EndsWith(".pximg.net")) {
							httpreq.Referer = "https://app-api.pixiv.net/";
						} else {
							httpreq.UserAgent = "CrosspostSharp/3.0 (https://github.com/libertyernie/CrosspostSharp)";
						}
					}
					using (var resp = await req.GetResponseAsync())
					using (var stream = resp.GetResponseStream())
					using (var ms = new MemoryStream()) {
						await stream.CopyToAsync(ms);
						ms.Position = 0;
						splitContainer1.Panel1.BackgroundImage = Image.FromStream(ms);
						splitContainer1.Panel1.BackgroundImageLayout = ImageLayout.Zoom;
						_data = ms.ToArray();
						_originalUrl = wrapper.ViewURL;
					}
				} catch (Exception ex) {
					splitContainer1.Panel1.Controls.Add(new TextBox {
						Text = ex.Message + Environment.NewLine + ex.StackTrace,
						Multiline = true,
						Dock = DockStyle.Fill,
						ReadOnly = true
					});
				}
			};
		}

		private void btnPost_Click(object sender, EventArgs e) {
			using (var f = new Form()) {
				f.Width = 600;
				f.Height = 350;
				var d = new DeviantArtUploadControl {
					Dock = DockStyle.Fill
				};
				f.Controls.Add(d);
				d.Uploaded += url => f.Close();
				d.SetSubmission(_data, txtTitle.Text, txtDescription.Text, txtTags.Text.Split(' '), chkPotentiallySensitiveMaterial.Checked, _originalUrl);
				f.ShowDialog(this);
			}
		}
	}
}
