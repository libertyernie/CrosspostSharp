using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.ParameterTypes;
using System.Collections.Generic;
using System.Linq;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtGallerySource : DeviantArtSource {
		public DeviantArtGallerySource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "DeviantArt";

		public override async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			await foreach (var deviation in DeviantArtFs.Api.Gallery.GetAllViewAsync(_token, UserScope.ForCurrentUser, PagingLimit.NewPagingLimit(24), PagingOffset.StartingOffset)) {
				var mr = await DeviantArtFs.Api.Deviation.GetMetadataAsync(
					_token,
					new[] { deviation.deviationid });
				yield return new DeviantArtPostWrapper(deviation, mr.metadata.Single());
			}
		}
	}
}
