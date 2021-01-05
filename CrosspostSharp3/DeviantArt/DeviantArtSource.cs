using ArtworkSourceSpecification;
using DeviantArtFs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrosspostSharp3.DeviantArt {
	public abstract class DeviantArtSource : IArtworkSource {
		protected readonly IDeviantArtAccessToken _token;

		public DeviantArtSource(IDeviantArtAccessToken token) {
			_token = token ?? throw new ArgumentNullException(nameof(token));
		}

		public abstract string Name { get; }

		public abstract IAsyncEnumerable<IPostBase> GetPostsAsync();

		private record Author : IAuthor {
			public string Name { get; init; }
			public string IconUrl { get; init; }
		}

		public async Task<IAuthor> GetUserAsync() {
			var user = await DeviantArtFs.Api.User.Whoami.ExecuteAsync(_token, DeviantArtObjectExpansion.None);
			return new Author {
				Name = user.username,
				IconUrl = user.usericon
			};
		}
	}
}
