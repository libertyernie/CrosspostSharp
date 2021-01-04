using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrosspostSharp3.FurryNetwork {
	public class FurryNetworkArtworkSource : IArtworkSource {
		private readonly FurryNetworkClient _client;
		private readonly string _characterName;

		public FurryNetworkArtworkSource(FurryNetworkClient client, string characterName = null) {
			_client = client;
			_characterName = characterName;
		}

		public string Name => "Furry Network";

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();
			string characterName = user.Name;

			int offset = 0;
			while (true) {
				var searchResults = await _client.SearchByCharacterAsync(characterName, new[] { "artwork" }, from: offset);
				foreach (var h in searchResults.Hits) {
					if (h.Submission is Artwork a) {
						yield return a;
					} else if (h.Submission is Photo p) {
						yield return p;
					}
				}

				offset += searchResults.Hits.Count();
				if (offset >= searchResults.Total)
					break;
			}
		}

		public async Task<IAuthor> GetUserAsync() {
			var user = await _client.GetUserAsync();
			return user.characters
				.Where(c => c.Name == _characterName)
				.DefaultIfEmpty(user.DefaultCharacter)
				.First();
		}
	}
}
