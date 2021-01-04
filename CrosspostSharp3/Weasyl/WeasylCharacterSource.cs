using ArtworkSourceSpecification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrosspostSharp3.Weasyl {
	public class WeasylCharacterSource : IArtworkSource {
		private readonly WeasylClient _client;

		public WeasylCharacterSource(WeasylClient client) {
			_client = client;
		}

		public string Name => "Weasyl (characters)";

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await _client.WhoamiAsync();
			var ids = await Scraper.GetCharacterIdsAsync(user.login);
			foreach (int id in ids) {
				yield return await _client.GetCharacterAsync(id);
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
