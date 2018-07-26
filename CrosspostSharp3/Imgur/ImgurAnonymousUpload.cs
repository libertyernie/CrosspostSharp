using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CrosspostSharp3.Imgur {
	public static class ImgurAnonymousUpload {
		public static async Task<string> UploadAsync(byte[] data, string title = null, string description = null) {
			var imgur = new ImgurClient(OAuthConsumer.Imgur.CLIENT_ID, OAuthConsumer.Imgur.CLIENT_SECRET);
			var endpoint = new ImageEndpoint(imgur);
			var image = await endpoint.UploadImageBinaryAsync(data, title: title, description: description);

			try {
				using (var fs = new FileStream("imgur-uploads.txt", FileMode.Append, FileAccess.Write))
				using (var sw = new StreamWriter(fs)) {
					await sw.WriteLineAsync($"{image.Link} {image.DeleteHash}");
				}
			} catch (Exception) { }

			return image.Link;
		}
	}
}
