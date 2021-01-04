using ArtworkSourceSpecification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrosspostSharp3.Weasyl {
	public class WeasylGallerySource : IArtworkSource {
		private readonly WeasylClient _client;

		public WeasylGallerySource(WeasylClient client) {
			_client = client;
		}

		public string Name => "Weasyl";

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await _client.WhoamiAsync();
			var gallery = await _client.GetUserGalleryAsync(user.login, new WeasylClient.GalleryRequestOptions());
			while (true) {
				foreach (var s in gallery.submissions) {
					var submission = await _client.GetSubmissionAsync(s.submitid);
					yield return submission;
				}

				if (gallery.nextid == null)
					break;

				gallery = await _client.GetUserGalleryAsync(user.login, new WeasylClient.GalleryRequestOptions { nextid = gallery.nextid });
			}
		}

		private record Author : IAuthor {
			public string Name { get; init; }
			public string IconUrl { get; init; }
		}

		public async Task<IAuthor> GetUserAsync() {
			var user = await _client.WhoamiAsync();
			return new Author {
				Name = user.login,
				IconUrl = await _client.GetAvatarUrlAsync(user.login)
			};
		}
	}
}
