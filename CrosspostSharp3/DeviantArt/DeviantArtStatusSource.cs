using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using DeviantArtFs.ParameterTypes;
using DeviantArtFs.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtStatusSource : DeviantArtSource {
		public DeviantArtStatusSource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "DeviantArt (statuses)";

		private class DeviantArtStatusPostWrapper : IPostBase {
			protected readonly Status _status;

			public DeviantArtStatusPostWrapper(Status status) {
				_status = status;
			}

			public string Title => "";
			public string HTMLDescription => _status.body.OrNull() ?? "";
			public bool Mature => _status.items.OrEmpty().Any(x => x.deviation.OrNull() is Deviation d && d.is_mature.IsTrue());
			public bool Adult => false;
			public IEnumerable<string> Tags => Enumerable.Empty<string>();
			public DateTime Timestamp => DateTime.UtcNow;
			public string ViewURL => _status.url.OrNull();
		}

		private class DeviantArtStatusPhotoPostWrapper : DeviantArtStatusPostWrapper, IRemotePhotoPost {
			private readonly Deviation _deviation;

			public DeviantArtStatusPhotoPostWrapper(Status status, Deviation deviation) : base(status) {
				_deviation = deviation;
			}

			public string UserIcon => _deviation.author.OrNull() is User u
				? u.usericon
				: "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif";

			public string ImageURL => _deviation.content.OrNull() is Content c
				? c.src
				: UserIcon;
			public string ThumbnailURL => _deviation.thumbs.OrEmpty()
				.Select(t => t.src)
				.DefaultIfEmpty(ImageURL)
				.First();
		}

		public override async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();
			var asyncEnum = DeviantArtFs.Api.User.GetStatusesAsync(_token, user.Name, PagingLimit.MaximumPagingLimit, PagingOffset.StartingOffset);
			await foreach (var s in asyncEnum) {
				var deviations = s.items.OrEmpty()
					.Select(x => x.deviation.OrNull())
					.Where(d => d != null);
				foreach (var d in deviations)
					yield return new DeviantArtStatusPhotoPostWrapper(s, d);
				if (!deviations.Any())
					yield return new DeviantArtStatusPostWrapper(s);
			}
		}
	}
}
