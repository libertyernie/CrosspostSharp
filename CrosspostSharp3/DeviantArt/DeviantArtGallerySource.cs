using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using DeviantArtFs.ParameterTypes;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtGallerySource : DeviantArtSource {
		public DeviantArtGallerySource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "DeviantArt";

		public override async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();

			var offset = PagingOffset.StartingOffset;
			while (true) {
				var deviations = await DeviantArtFs.Api.Gallery.AsyncPageAllView(
					_token,
					UserScope.ForCurrentUser,
					PagingLimit.NewPagingLimit(24),
					offset).StartAsTask();

				var metadata = await DeviantArtFs.Api.Deviation.AsyncGetMetadata(
					_token,
					ExtParams.None,
					deviations.results.OrEmpty().Select(x => x.deviationid)).StartAsTask();

				foreach (var d in deviations.results.OrEmpty()) {
					var m = metadata.metadata.Single(x => x.deviationid == d.deviationid);
					yield return new DeviantArtPostWrapper(d, m);
				}

				if (!deviations.has_more)
					break;

				offset = PagingOffset.NewPagingOffset(deviations.next_offset.Value);
			}
		}
	}
}
