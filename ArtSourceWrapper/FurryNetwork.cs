using FurryNetworkLib;
using Pixeez;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class FurryNetworkWrapper : SiteWrapper<FurryNetworkSubmissionWrapper, int> {
		private FurryNetworkClient _client;
		public Character _character;

		public FurryNetworkWrapper(FurryNetworkClient client) {
			_client = client;
		}

		private async Task<Character> GetCharacter() {
			if (_character == null) {
				var user = await _client.GetUserAsync();
				_character = user.DefaultCharacter;
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
			return character.Avatar;
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
				searchResults.Hits
					.Select(h => h.Submission)
					.Where(h => h is Artwork || h is Photo)
					.Select(h => new FurryNetworkSubmissionWrapper((FileSubmission)h))
					.ToList(),
				nextPosition,
				nextPosition >= searchResults.Total);
		}
	}

	public class FurryNetworkSubmissionWrapper : ISubmissionWrapper {
		private FileSubmission _artwork;

		public FurryNetworkSubmissionWrapper(FileSubmission artwork) {
			_artwork = artwork;
		}

		public string Title => _artwork.Title;
		public string HTMLDescription => _artwork.Description; // TODO: parse Markdown
		public bool PotentiallySensitive => _artwork.Rating != 0;
		public IEnumerable<string> Tags => _artwork.Tags;
		public DateTime Timestamp => _artwork.Created;
		public string ViewURL => $"https://beta.furrynetwork.com/artwork/{_artwork.Id}";
		public string ImageURL => _artwork.Images.Original;
		public string ThumbnailURL => _artwork.Images.Thumbnail;
		public Color? BorderColor =>
			_artwork.Rating == 0 ? (Color?)null
			: _artwork.Rating == 1 ? Color.FromArgb(0xFF, 0xFD, 0xD8, 0x35)
			: Color.FromArgb(0xFF, 0xDD, 0x2c, 0x00);
		public bool OwnWork => true;
	}
}
