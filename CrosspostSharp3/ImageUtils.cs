using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public static class ImageUtils {
		public static byte[] MakeSquare(byte[] imageData) {
			using (var ms1 = new MemoryStream(imageData, false))
			using (var image1 = Image.FromStream(ms1)) {
				int size = Math.Max(image1.Width, image1.Height);
				using (var image2 = new Bitmap(size, size))
				using (var g2 = Graphics.FromImage(image2))
				using (var ms2 = new MemoryStream()) {
					g2.DrawImage(image1,
						(image2.Width - image1.Width) / 2,
						(image2.Height - image1.Height) / 2,
						image1.Width,
						image1.Height);
					image2.Save(ms2, image1.RawFormat);
					return ms2.ToArray();
				}
			}
		}
	}
}
