using ArtworkSourceSpecification;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrosspostSharp3.Tumblr {
	public class TumblrSource : IArtworkSource {
		private readonly TumblrClient _client;
		private readonly string _blogName;
		private readonly PostType _type;

		public TumblrSource(TumblrClient client, string blogName, PostType type = PostType.All) {
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_blogName = blogName ?? throw new ArgumentNullException(nameof(blogName));
			_type = type;
		}

		public string Name => _type == PostType.All
			? "Tumblr"
			: $"Tumblr ({_type})";

		public abstract class TumblrPostWrapper<T> : IPostBase where T : BasePost {
			private readonly T _post;

			public abstract string HTMLDescription { get; }

			protected TumblrPostWrapper(T post) {
				_post = post;
			}

			public string Title => "";
			public bool Mature => false;
			public bool Adult => false;
			public IEnumerable<string> Tags => _post.Tags;
			public DateTime Timestamp => _post.Timestamp;
			public string ViewURL => _post.Url;
		}

		public class TumblrPhotoPostWrapper : TumblrPostWrapper<PhotoPost>, IRemotePhotoPost {
			private readonly PhotoPost _post; 
			
			public TumblrPhotoPostWrapper(PhotoPost post) : base(post) {
				_post = post;
			}

			public override string HTMLDescription => _post.Caption;
			public string ImageURL => _post.Photo.OriginalSize.ImageUrl;
			public string ThumbnailURL => _post.Photo.AlternateSizes
				.OrderBy(x => x.Width)
				.Where(x => x.Width >= 120 && x.Height >= 120)
				.Select(x => x.ImageUrl)
				.DefaultIfEmpty(ImageURL)
				.First();
		}

		public class TumblrTextPostWrapper : TumblrPostWrapper<DontPanic.TumblrSharp.Client.TextPost> {
			private readonly DontPanic.TumblrSharp.Client.TextPost _post;

			public TumblrTextPostWrapper(DontPanic.TumblrSharp.Client.TextPost post) : base(post) {
				_post = post;
			}

			public override string HTMLDescription => _post.Body;
		}

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var posts = await _client.GetPostsAsync(_blogName, type: _type, includeReblogInfo: true);
			while (posts.Result.Any()) {
				foreach (var post in posts.Result) {
					string blogName = post.RebloggedRootName ?? post.BlogName;
					if (blogName == _blogName) {
						if (post is PhotoPost p)
							yield return new TumblrPhotoPostWrapper(p);
						else if (post is DontPanic.TumblrSharp.Client.TextPost t)
							yield return new TumblrTextPostWrapper(t);
					}
				}
			}
		}

		public record Author : IAuthor {
			public string Name { get; init; }
			public string IconUrl { get; init; }
		}

		public Task<IAuthor> GetUserAsync() {
			string hostname = _blogName.Contains('.')
				? _blogName
				: _blogName + ".tumblr.com";
			return Task.FromResult<IAuthor>(new Author {
				Name = _blogName,
				IconUrl = $"https://api.tumblr.com/v2/blog/{hostname}/avatar/64"
			});
		}
	}
}
