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

		public class PostWrapper : IRemotePhotoPost, IDeletable {
			private readonly FurryNetworkClient _client;
			private readonly FileSubmission _artwork;

			public PostWrapper(FurryNetworkClient client, FileSubmission artwork) {
				_client = client;
				_artwork = artwork;
			}

			public string Title => _artwork.Title;
			public string HTMLDescription => CommonMark.CommonMarkConverter.Convert(_artwork.Description);
			public bool Mature => _artwork.Rating == 1;
			public bool Adult => _artwork.Rating >= 2;
			public IEnumerable<string> Tags => _artwork.TagStrings;
			public DateTime Timestamp => _artwork.Created;
			public string ViewURL => $"https://furrynetwork.com/artwork/{_artwork.Id}";
			public string ImageURL => _artwork.Images.Original;
			public string ThumbnailURL => _artwork.Images.Thumbnail;
			public string SiteName => "Furry Network";

			public async Task DeleteAsync() {
				await _client.DeleteArtwork(_artwork.Id);
			}
		}

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();
			string characterName = user.Name;

			int offset = 0;
			while (true) {
				var searchResults = await _client.SearchByCharacterAsync(characterName, new[] { "artwork" }, from: offset);
				foreach (var h in searchResults.Hits) {
					if (h.Submission is Artwork a) {
						yield return new PostWrapper(_client, a);
					} else if (h.Submission is Photo p) {
						yield return new PostWrapper(_client, p);
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
