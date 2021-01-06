using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrosspostSharp3.DeviantArt {
	public class StashSource : DeviantArtSource {
		public StashSource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "Sta.sh";

		public class StashPostWrapper : IRemotePhotoPost {
			public readonly long ItemId;

			private readonly StashMetadata _metadata;

			public StashPostWrapper(long itemId, StashMetadata metadata) {
				ItemId = itemId;
				_metadata = metadata;
			}

			public string ImageURL => _metadata.files.OrEmpty()
				.Where(f => f.width * f.height > 0)
				.OrderByDescending(f => f.width)
				.Select(f => f.src)
				.DefaultIfEmpty("https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif")
				.First();
			public string ThumbnailURL => _metadata.thumb.OrNull() is DeviationPreview p
				? p.src
				: ImageURL;
			public string Title => _metadata.title;
			public string HTMLDescription => _metadata.artist_comments.OrNull() ?? "";
			public bool Mature => false;
			public bool Adult => false;
			public IEnumerable<string> Tags => _metadata.tags.OrEmpty();
			public DateTime Timestamp => _metadata.creation_time.OrNull() is DateTimeOffset d
				? d.UtcDateTime
				: DateTime.UtcNow;

			public string ViewURL {
				get {
					var url = new StringBuilder();
					long working = ItemId;
					while (working > 0) {
						int n = (int)(working % 36);
						char c = "0123456789abcdefghijklmnopqrstuvwxyz"[n];
						url.Insert(0, c);
						working /= 36;
					}
					url.Insert(0, "https://sta.sh/0");
					return url.ToString();
				}
			}
		}

		public async IAsyncEnumerable<IPostBase> GetPostsUnorderedAsync() {
			var req = new DeviantArtFs.Api.Stash.DeltaRequest();
			var all = await DeviantArtFs.Api.Stash.Delta.ToArrayAsync(_token, req, 0, int.MaxValue);
			foreach (var item in all) {
				if (item.itemid.OrNull() is long itemid && item.metadata.OrNull() is StashMetadata metadata) {
					yield return new StashPostWrapper(itemid, metadata);
				}
			}
		}

		public override IAsyncEnumerable<IPostBase> GetPostsAsync() =>
			GetPostsUnorderedAsync().OrderByDescending(x => x.Timestamp);
	}
}
