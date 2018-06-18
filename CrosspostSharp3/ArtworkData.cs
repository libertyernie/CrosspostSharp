using Newtonsoft.Json;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public struct ArtworkData {
		public byte[] data;
		public string contentType;
		public string title, description, url;
		public IEnumerable<string> tags;
		public bool mature;
		public bool adult;

		public static async Task<ArtworkData> DownloadAsync(IPostWrapper wrapper) {
			if (wrapper.ImageURL == null) {
				throw new NotImplementedException("Cannot create a post without an image.");
			}
			var req = WebRequestFactory.Create(wrapper.ImageURL);
			using (var resp = await req.GetResponseAsync())
			using (var stream = resp.GetResponseStream())
			using (var ms = new MemoryStream()) {
				await stream.CopyToAsync(ms);
				return new ArtworkData {
					data = ms.ToArray(),
					contentType = resp.ContentType,
					title = wrapper.Title,
					description = wrapper.HTMLDescription,
					tags = wrapper.Tags,
					mature = wrapper.Mature,
					adult = wrapper.Adult,
					url = wrapper.ViewURL
				};
			}
		}

		public static ArtworkData FromFile(string filename) {
			var data = File.ReadAllBytes(filename);
			if (data[0] == '{') {
				try {
					return JsonConvert.DeserializeObject<ArtworkData>(Encoding.UTF8.GetString(data));
				} catch (Exception ex) {
					Console.Error.WriteLine($"Cannot read {filename} as JSON: {ex.Message}");
					Console.Error.WriteLine(ex.StackTrace);
				}
			}
			return new ArtworkData {
				data = data,
				contentType = GetContentType(data),
				title = Path.GetFileName(filename),
				description = "",
				tags = Enumerable.Empty<string>(),
				mature = false,
				url = null
			};
		}

		private static string GetContentType(byte[] data) {
			try {
				using (var ms = new MemoryStream(data, false)) {
					var image = Image.FromStream(ms);
					if (image.RawFormat.Guid == ImageFormat.Png.Guid) return "image/png";
					if (image.RawFormat.Guid == ImageFormat.Jpeg.Guid) return "image/jpeg";
					if (image.RawFormat.Guid == ImageFormat.Gif.Guid) return "image/gif";
				}
			} catch (Exception) { }
			return "application/octet-stream";
		}

		public string GetContentType() {
			return contentType;
		}

		public string GetFileName() {
			char[] invalid = Path.GetInvalidFileNameChars();
			string title = new string(this.title.Where(c => !invalid.Contains(c)).ToArray());
			string md5 = string.Join("", MD5.Create().ComputeHash(data).Select(b => ((int)b).ToString("x2")));
			return $"{title} ({md5}).{GetContentType().Split('/').Last()}";
		}
	}
}
