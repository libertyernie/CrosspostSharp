using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CrosspostSharp3 {
	public record LocalPhotoPost(string Filename) : IDownloadedData {
		public byte[] Data => File.ReadAllBytes(Filename);

		public string ContentType {
			get {
				using var image = Image.FromFile(Filename);
				return image.RawFormat.Guid == ImageFormat.Png.Guid ? "image/png"
					: image.RawFormat.Guid == ImageFormat.Jpeg.Guid ? "image/jpeg"
					: image.RawFormat.Guid == ImageFormat.Gif.Guid ? "image/gif"
					: "application/octet-stream";
			}
		}
	}
}
