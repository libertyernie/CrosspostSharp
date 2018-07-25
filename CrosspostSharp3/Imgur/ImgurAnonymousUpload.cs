using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3.Imgur {
	public static class ImgurAnonymousUpload {
		public static async Task<string> UploadAsync(byte[] data) {
			var imgur = new ImgurClient(OAuthConsumer.Imgur.CLIENT_ID, OAuthConsumer.Imgur.CLIENT_SECRET);
			var endpoint = new ImageEndpoint(imgur);
			var image = await endpoint.UploadImageBinaryAsync(data);

			try {
				using (var fs = new FileStream("imgur-uploads.txt", FileMode.Append, FileAccess.Write))
				using (var sw = new StreamWriter(fs)) {
					await sw.WriteLineAsync($"{image.Link} {image.DeleteHash}");
				}
			} catch (Exception) { }

			return image.Link;
		}

		public static async Task PromptForDeletionAsync(string url) {
			if (File.Exists("imgur-uploads.txt")) {
				using (var fs = new FileStream("imgur-uploads.txt", FileMode.Open, FileAccess.Read))
				using (var sr = new StreamReader(fs)) {
					string line;
					while ((line = await sr.ReadLineAsync()) != null) {
						string[] split = line.Split(' ');
						if (url == split[0]) {
							string deletehash = split[1];
							if (MessageBox.Show(null, $"Your submission has been deleted. Would you also like to delete it from Imgur?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
								var imgur = new ImgurClient(OAuthConsumer.Imgur.CLIENT_ID, OAuthConsumer.Imgur.CLIENT_SECRET);
								var endpoint = new ImageEndpoint(imgur);
								await endpoint.DeleteImageAsync(deletehash);
								break;
							}
						}
					}
				}
			}
		}
	}
}
