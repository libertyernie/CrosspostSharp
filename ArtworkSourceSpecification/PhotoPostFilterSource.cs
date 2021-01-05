using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSourceSpecification {
	public class PhotoPostFilterSource : IArtworkSource {
		private readonly IArtworkSource _source;

		public string Name => $"{_source.Name} (images only)";

		public PhotoPostFilterSource(IArtworkSource source) {
			_source = source;
		}

		public Task<IAuthor> GetUserAsync() => _source.GetUserAsync();

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			await foreach (var p in _source.GetPostsAsync()) {
				if (p is IRemotePhotoPost)
					yield return p;
			}
		}
	}
}
