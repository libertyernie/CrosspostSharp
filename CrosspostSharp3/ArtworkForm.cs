using ArtSourceWrapper;
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
	}
}
