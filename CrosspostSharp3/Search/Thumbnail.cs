using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtSourceWrapper;
using System.Diagnostics;
using System.IO;

namespace CrosspostSharp3.Search {
	public partial class Thumbnail : UserControl {
		public Thumbnail(ISubmissionWrapper wrapper) {
			InitializeComponent();
			linkLabel1.Text = wrapper.Title;
			
			LoadImage(wrapper.ThumbnailURL);
		}

		private async void LoadImage(string url) {
			try {
				var req = WebRequestFactory.Create(url);
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					panel1.BackgroundImage = Image.FromStream(ms);
				}
			} catch (Exception) { }
		}
	}
}
