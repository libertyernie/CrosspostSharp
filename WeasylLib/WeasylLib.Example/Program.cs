using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WeasylLib.Example {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("You can get a Weasyl API key from: https://www.weasyl.com/control/apikeys");
			Console.Write("Enter your API key: ");
			string apiKey = Console.ReadLine();
			if (string.IsNullOrEmpty(apiKey)) return;

			var client = new WeasylClient(apiKey);
			PrintAvatar(client).GetAwaiter().GetResult();
		}

		static async Task UploadImageAsync(WeasylClient c) {
			var folders = await c.GetFoldersAsync();
			var uri = await c.UploadVisualAsync(
				File.ReadAllBytes(@"C:\Users\admin\Pictures\san diego\IMG_2654.jpg"),
				"San Diego 2017",
				WeasylClient.SubmissionType.Photography,
				folders.First().FolderId,
				WeasylClient.Rating.General,
				"Uploading some old pictures to test some C# code I'm writing.",
				new[] { "photo", "baseball", "san diego" });
			Console.WriteLine(uri);
		}

		static async Task PrintAvatar(WeasylClient client) {
			var user = await client.WhoamiAsync();
			string url = await client.GetAvatarUrlAsync(user.login);
			var request = WebRequest.Create(url);
			using (var response = await request.GetResponseAsync())
			using (var stream = response.GetResponseStream()) {
				if (Image.FromStream(stream) is Bitmap bmp) {
					ConsoleImage.ConsoleWriteImage(bmp);
				}
			}
		}

		static async Task ListGallery(WeasylClient client) {
			var user = await client.WhoamiAsync();
			Console.WriteLine(user.login);

			Console.WriteLine("----------");
			var gallery = await client.GetUserGalleryAsync(user.login, new WeasylClient.GalleryRequestOptions {
				count = 1
			});
			foreach (var s in gallery.submissions) Console.WriteLine(s.title);

			Console.WriteLine("----------");
			gallery = await client.GetUserGalleryAsync(user.login, new WeasylClient.GalleryRequestOptions {
				count = 10,
				nextid = gallery.nextid
			});
			foreach (var s in gallery.submissions) Console.WriteLine(s.title);

			Console.WriteLine("----------");
			gallery = await client.GetUserGalleryAsync(user.login, new WeasylClient.GalleryRequestOptions {
				count = 3,
				nextid = gallery.nextid
			});
			foreach (var s in gallery.submissions) Console.WriteLine(s.title);

			Console.WriteLine("----------");
			foreach (var s in gallery.submissions) {
				var details = await client.GetSubmissionAsync(s.submitid);
				Console.WriteLine(details.title + ":");
				Console.WriteLine(details.description);
				Console.WriteLine();
			}
		}

		static async Task ListCharacters(WeasylClient client) {
			var user = await client.WhoamiAsync();
			Console.WriteLine(user.login);

			Console.WriteLine("----------");
			var charids = await Scraper.GetCharacterIdsAsync(user.login);
			foreach (int id in charids) Console.WriteLine(id);

			Console.WriteLine("----------");
			foreach (int id in charids.Take(3)) {
				var details = await client.GetCharacterAsync(id);
				Console.WriteLine(details.title);
				Console.WriteLine("Species: " + details.species);
				Console.WriteLine();
			}
		}
	}
}
