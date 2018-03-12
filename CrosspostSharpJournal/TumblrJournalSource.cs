using ArtSourceWrapper;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharpJournal {
	public class TumblrJournalSource : JournalSource<TumblrJournalWrapper, long> {
		private readonly TumblrClient _client;
		private string _blogName;
		private IEnumerable<string> _blogNames;

		public TumblrJournalSource(TumblrClient client, string blogName) {
			_client = client;
			_blogName = blogName;
		}

		public override int BatchSize { get; set; } = 20;
		public override int MinBatchSize => 1;
		public override int MaxBatchSize => 20;

		protected override async Task<InternalFetchResult> InternalFetchAsync(long? startPosition, int maxCount) {
			if (_blogNames == null) {
				var user = await _client.GetUserInfoAsync();
				_blogNames = user.Blogs.Select(b => b.Name).ToList();
			}
			if (!_blogNames.Contains(_blogName)) {
				throw new TumblrWrapperException($"The blog {_blogName} does not appear to be owned by the currently logged in user. (Make sure the name is spelled and capitalized correctly.)");
			}

			long position = startPosition ?? 0;

			var posts = await _client.GetPostsAsync(
				_blogName,
				position,
				count: maxCount,
				type: PostType.Text,
				includeReblogInfo: true);

			if (!posts.Result.Any()) {
				return new InternalFetchResult(position, isEnded: true);
			}

			position += posts.Result.Length;

			var list = posts.Result
				.Select(post => post as TextPost)
				.Where(post => post != null)
				.Where(post => _blogNames.Contains(post.RebloggedRootName ?? post.BlogName))
				.Select(post => new TumblrJournalWrapper(post));

			return new InternalFetchResult(list, position);
		}
	}

	public class TumblrJournalWrapper : IJournalWrapper {
		public readonly TextPost Post;

		public TumblrJournalWrapper(TextPost post) {
			Post = post;
		}

		public string Title => Post.Title;
		public string HTMLDescription => Post.Body;
		public DateTime Timestamp => Post.Timestamp;
		public string ViewURL => Post.Url;
	}
}
