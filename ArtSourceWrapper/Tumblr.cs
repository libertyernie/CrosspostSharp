using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class TumblrWrapperException : Exception {
        public TumblrWrapperException(string message) : base(message) { }
    }

    public class TumblrWrapper : SiteWrapper<TumblrSubmissionWrapper, long> {
        private readonly TumblrClient _client;
        private string _blogName;
        private IEnumerable<string> _blogNames;

        public TumblrWrapper(TumblrClient client, string blogName) {
            _client = client;
            _blogName = blogName;
        }

        public override string SiteName => "Tumblr";
        public override string WrapperName => "Tumblr";

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
                type: PostType.Photo,
                includeReblogInfo: true);

            if (!posts.Result.Any()) {
                return new InternalFetchResult(position, isEnded: true);
            }

            position += posts.Result.Length;

            var list = posts.Result
                .Select(post => post as PhotoPost)
                .Where(post => post != null)
                .Where(post => _blogNames.Contains(post.RebloggedRootName ?? post.BlogName))
                .Select(post => new TumblrSubmissionWrapper(post));

            return new InternalFetchResult(list, position);
        }

        public override async Task<string> WhoamiAsync() {
            var user = await _client.GetUserInfoAsync();
            return user.Name;
        }

        public override async Task<string> GetUserIconAsync(int size) {
            return $"https://api.tumblr.com/v2/blog/{_blogName}.tumblr.com/avatar/{size}";
        }
    }

    public class TumblrSubmissionWrapper : ISubmissionWrapper {
		public readonly PhotoPost Post;
        
		public bool OwnWork => true;

		public TumblrSubmissionWrapper(PhotoPost post) {
            Post = post;
        }

        public string Title => "";
        public string HTMLDescription => Post.Caption;
        public bool PotentiallySensitive => false;
        public IEnumerable<string> Tags => Post.Tags;
        public DateTime Timestamp => Post.Timestamp;
        public string ViewURL => Post.Url;
        public string ImageURL => Post.Photo.OriginalSize.ImageUrl;
        public string ThumbnailURL {
            get {
				foreach (var alt in Post.Photo.AlternateSizes.OrderBy(s => s.Width)) {
                    if (alt.Width < 120 && alt.Height < 120) continue;
                    return alt.ImageUrl;
                }
                return Post.Photo.OriginalSize.ImageUrl;
            }
        }
        public Color? BorderColor => null;
	}
}
