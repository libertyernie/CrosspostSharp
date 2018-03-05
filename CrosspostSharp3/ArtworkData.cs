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
				title = Path.GetFileName(filename),
				description = "",
				tags = Enumerable.Empty<string>(),
				mature = false,
				url = null
			};
		}
	}
}
