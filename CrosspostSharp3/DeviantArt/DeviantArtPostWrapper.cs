using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtPostWrapper : IRemotePhotoPost {
		private readonly Deviation _deviation;
		private readonly DeviationMetadata _metadata;

		public DeviantArtPostWrapper(Deviation deviation, DeviationMetadata metadata) {
			_deviation = deviation;
			_metadata = metadata;
		}

		public string ImageURL => _deviation.content.OrNull() is DeviationContent c
			? c.src
			: "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif";
		public string ThumbnailURL => _deviation.thumbs.OrEmpty()
			.Select(x => x.src)
			.DefaultIfEmpty(ImageURL)
			.First();
		public string Title => _deviation.title.OrNull() ?? "";
		public string HTMLDescription => _metadata.description.Replace("https://www.deviantart.com/users/outgoing?", "");
		public bool Mature => _metadata.is_mature == true;
		public bool Adult => false;
		public IEnumerable<string> Tags => _metadata.tags.Select(t => t.tag_name);
		public DateTime Timestamp => _deviation.published_time.OrNull() is DateTimeOffset dt
			? dt.UtcDateTime
			: DateTime.UtcNow;
		public string ViewURL => _deviation.url.OrNull() ?? "https://www.example.com";
	}
}
