using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace CrosspostSharp3 {
	public record LocalPhotoPost : IPostBase, IDownloadedData {
		public byte[] Data { get; init; }
		public string Filename { get; init; }

		public string ContentType {
			get {
				using var ms = new MemoryStream(Data, false);
				using var image = Image.FromStream(ms);
				return image.RawFormat.Guid == ImageFormat.Png.Guid ? "image/png"
					: image.RawFormat.Guid == ImageFormat.Jpeg.Guid ? "image/jpeg"
					: image.RawFormat.Guid == ImageFormat.Gif.Guid ? "image/gif"
					: "application/octet-stream";
			}
		}

		public string Title => Filename;
		public string HTMLDescription => "";
		public bool Mature => false;
		public bool Adult => false;
		public IEnumerable<string> Tags => Enumerable.Empty<string>();
		public DateTime Timestamp => DateTime.UtcNow;
		public string ViewURL => "about:blank";

		public static LocalPhotoPost FromFile(string path) => new LocalPhotoPost {
			Data = File.ReadAllBytes(path),
			Filename = Path.GetFileName(path)
		};
	}
}
