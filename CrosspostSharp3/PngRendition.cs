using SourceWrappers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CrosspostSharp3 {
	public class PngRendition : IDownloadedData {
		public PngRendition(IDownloadedData post) {
			using var ms = new MemoryStream(post.Data, false);
			using var image = Image.FromStream(ms);
			using var ms2 = new MemoryStream();
			image.Save(ms2, ImageFormat.Png);

			Data = ms2.ToArray();
			ContentType = "image/png";
			Filename = $"{Path.GetFileNameWithoutExtension(post.Filename)}.png";
		}

		public byte[] Data { get; }
		public string ContentType { get; }
		public string Filename { get; }
	}
}
