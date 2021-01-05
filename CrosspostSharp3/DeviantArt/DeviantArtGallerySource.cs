using ArtworkSourceSpecification;
using DeviantArtFs;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtGallerySource : DeviantArtSource {
		public DeviantArtGallerySource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "DeviantArt";

		public override async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();

			var req = new DeviantArtFs.Api.Gallery.GalleryAllViewRequest { Username = user.Name };
			var paging = new DeviantArtPagingParams(0, 24);
			while (true) {
				var deviations = await DeviantArtFs.Api.Gallery.GalleryAllView.ExecuteAsync(_token, req, paging);

				var metadata = await DeviantArtFs.Api.Deviation.MetadataById.ExecuteAsync(
					_token,
					new DeviantArtFs.Api.Deviation.MetadataRequest(deviations.results.Select(x => x.deviationid)));

				foreach (var d in deviations.results) {
					var m = metadata.metadata.Single(x => x.deviationid == d.deviationid);
					yield return new DeviantArtPostWrapper(d, m);
				}

				if (!deviations.has_more)
					break;

				paging = new DeviantArtPagingParams(deviations.next_offset.Value, 24);
			}
		}
	}
}
