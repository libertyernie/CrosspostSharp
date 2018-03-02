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
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			ddlSource.Items.Add(new FurAffinityWrapper(new FurAffinityIdWrapper(
				b: "",
				a: "")));
		}

		private async void btnLoad_Click(object sender, EventArgs e) {
			var wrapper = ddlSource.SelectedItem as ISiteWrapper;
			if (wrapper == null) return;

			while (!wrapper.IsEnded && wrapper.Cache.Count() < 4) {
				await wrapper.FetchAsync();
			}

			for (int i = 0; i < 4 && i < wrapper.Cache.Count(); i++) {
				var item = wrapper.Cache.Skip(i).First();

				Image image;
				var req = WebRequest.Create(item.ThumbnailURL);
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					image = Image.FromStream(ms);
				}

				//int size = Math.Max(image.Width, image.Height);
				//Image image2 = new Bitmap(size, size);
				//using (var g = Graphics.FromImage(image2)) {
				//	g.DrawImage(image, (size - image.Width) / 2, (size - image.Height) / 2, image.Width, image.Height);
				//}

				tableLayoutPanel1.Controls.Add(new Panel {
					BackgroundImage = image,
					BackgroundImageLayout = ImageLayout.Zoom,
					Dock = DockStyle.Fill
				});
			}
		}
	}
}
