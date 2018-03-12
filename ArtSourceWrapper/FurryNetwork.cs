using FurryNetworkLib;
using Pixeez;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class FurryNetworkWrapper : SiteWrapper<FurryNetworkSubmissionWrapper, int> {
		private FurryNetworkClient _client;
		private string _preferredCharacterName;
		public Character _character;

		public FurryNetworkWrapper(FurryNetworkClient client, string characterName = null) {
			_client = client;
			_preferredCharacterName = characterName;
		}

		private async Task<Character> GetCharacter() {
			if (_character == null) {
				var user = await _client.GetUserAsync();
				_character = user.characters.Where(c => c.Name == _preferredCharacterName).FirstOrDefault()
					?? user.DefaultCharacter;
			}
			return _character;
		}

		public override string SiteName => "Furry Network";

		public override string WrapperName => "Furry Network";

		public override int BatchSize { get; set; }

		public override int MinBatchSize => 30;

		public override int MaxBatchSize => 30;

		public override async Task<string> GetUserIconAsync(int size) {
			var character = await GetCharacter();
			string preferred = size <= 50 ? character.Avatars?.Tiny
				: size <= 80 ? character.Avatars?.Small
				: size <= 315 ? character.Avatars?.Avatar
				: character.Avatars?.Original;
			return preferred ?? character.Avatars?.GetLargest();
		}

		public override async Task<string> WhoamiAsync() {
			var character = await GetCharacter();
			return character.Name;
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			var character = await GetCharacter();
			var searchResults = await _client.SearchByCharacterAsync(character.Name, new[] { "artwork" }, from: startPosition ?? 0);
			int nextPosition = (startPosition ?? 0) + searchResults.Hits.Count();
			return new InternalFetchResult(
				await Task.WhenAll(searchResults.Hits
					.Select(h => h.Submission)
					.Where(h => h is Artwork || h is Photo)
					.Select(h => FurryNetworkSubmissionWrapper.CreateAsync((FileSubmission)h, _client))
					.ToArray()),
				nextPosition,
				nextPosition >= searchResults.Total);
		}
	}

	public class FurryNetworkSubmissionWrapper : ISubmissionWrapper, IDeletable {
		private int _id;
		private FurryNetworkClient _client;

		private FileSubmission _artwork;
		private string _html;

		private FurryNetworkSubmissionWrapper() { }

		public static async Task<FurryNetworkSubmissionWrapper> CreateAsync(FileSubmission artwork, FurryNetworkClient client = null) {
			string html = WebUtility.HtmlEncode(artwork.Description);

			try {
				html = CommonMark.CommonMarkConverter.Convert(artwork.Description);
			} catch (Exception) {}

			return new FurryNetworkSubmissionWrapper {
				_id = artwork.Id,
				_client = client,
				_artwork = artwork,
				_html = html
			};
		}

		public string Title => _artwork.Title;
		public string HTMLDescription => _html ?? _artwork.Description;
		public bool Mature => _artwork.Rating == 1;
		public bool Adult => _artwork.Rating >= 2;
		public IEnumerable<string> Tags => _artwork.Tags.Select(s => s?.ToString());
		public DateTime Timestamp => _artwork.Created;
		public string ViewURL => $"https://beta.furrynetwork.com/artwork/{_artwork.Id}";
		public string ImageURL => _artwork.Images.Original;
		public string ThumbnailURL => _artwork.Images.Thumbnail;
		public Color? BorderColor =>
			_artwork.Rating == 0 ? (Color?)null
			: _artwork.Rating == 1 ? Color.FromArgb(0xFF, 0xFD, 0xD8, 0x35)
			: Color.FromArgb(0xFF, 0xDD, 0x2c, 0x00);

		public string SiteName => "Furry Network";

		public async Task DeleteAsync() {
			await _client.DeleteArtwork(_id);
		}
	}
}
