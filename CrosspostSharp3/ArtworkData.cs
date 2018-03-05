using ArtSourceWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public struct ArtworkMetadata {
		public string imagePath;
		public string title, description, url;
		public IEnumerable<string> tags;
		public bool mature;
		//public ArtworkMetadataMaturityLevel nudity, violence;

		public ArtworkData Read(string relativeDirectory) {
			return new ArtworkData {
				data = File.ReadAllBytes(Path.Combine(relativeDirectory, imagePath)),
				title = title,
				description = description,
				tags = tags,
				mature = mature,
				url = url
			};
		}
	}

	//public struct ArtworkMetadataMaturityLevel {
	//	public bool moderate, explicit;
	//}

	public struct ArtworkData {
		public byte[] data;
		public string title, description, url;
		public IEnumerable<string> tags;
		public bool mature;
		//public ArtworkMetadataMaturityLevel nudity, violence;

		public static async Task<ArtworkData> DownloadAsync(ISubmissionWrapper wrapper) {
			var req = WebRequestFactory.Create(wrapper.ImageURL);
			using (var resp = await req.GetResponseAsync())
			using (var stream = resp.GetResponseStream())
			using (var ms = new MemoryStream()) {
				await stream.CopyToAsync(ms);
				return new ArtworkData {
					data = ms.ToArray(),
					title = wrapper.Title,
					description = wrapper.HTMLDescription,
					tags = wrapper.Tags,
					mature = wrapper.PotentiallySensitive,
					url = wrapper.ViewURL
				};
			}
		}

		public static ArtworkData FromFile(string filename) {
			return new ArtworkData {
				data = File.ReadAllBytes(filename),
				title = Path.GetFileName(filename),
				description = "",
				tags = Enumerable.Empty<string>(),
				mature = false,
				url = null
			};
		}

		public void Write(string jsonFile) {
			string ext;
			using (var ms = new MemoryStream(data, false))
			using (var image = Image.FromStream(ms)) {
				ext = image.RawFormat.Equals(ImageFormat.Png) ? ".png"
					: image.RawFormat.Equals(ImageFormat.Jpeg) ? ".jpeg"
					: image.RawFormat.Equals(ImageFormat.Gif) ? ".gif"
					: ".dat";
			}
			string imageFile = jsonFile + ext;
			File.WriteAllBytes(imageFile, data);
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(new ArtworkMetadata {
				description = description,
				imagePath = imageFile,
				mature = mature,
				tags = tags,
				title = title,
				url = url
			}));
		}
	}
}
