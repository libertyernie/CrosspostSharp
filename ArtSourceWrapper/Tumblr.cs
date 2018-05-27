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

    public class TumblrWrapper : SiteWrapper<ISubmissionWrapper, long> {
        private readonly TumblrClient _client;
		private readonly bool _photosOnly;
		private readonly string _blogName;
        private IEnumerable<string> _blogNames;

        public TumblrWrapper(TumblrClient client, string blogName, bool photosOnly = true) {
            _client = client;
            _blogName = blogName;
			_photosOnly = photosOnly;
        }
		
        public override string WrapperName => _photosOnly ? "Tumblr (photos)" : "Tumblr (text + photos)";
		public override bool SubmissionsFiltered => true;

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
                type: _photosOnly ? PostType.Photo : PostType.All,
                includeReblogInfo: true);

            if (!posts.Result.Any()) {
                return new InternalFetchResult(position, isEnded: true);
            }

            position += posts.Result.Length;
			
            return new InternalFetchResult(Wrap(posts.Result), position);
        }

		private IEnumerable<ISubmissionWrapper> Wrap(IEnumerable<BasePost> posts) {
			foreach (var post in posts) {
				if (_blogNames.Contains(post.RebloggedRootName ?? post.BlogName)) {
					if (post is PhotoPost p1) {
						yield return new DeletableTumblrSubmissionWrapper(_client, p1);
					} else if (post is TextPost p2) {
						yield return new TumblrTextPostSubmissionWrapper(p2);
					}
				}
			}
		}

        public override Task<string> WhoamiAsync() {
			return Task.FromResult(_blogName);
        }

        public override Task<string> GetUserIconAsync(int size) {
            return Task.FromResult($"https://api.tumblr.com/v2/blog/{_blogName}.tumblr.com/avatar/{size}");
        }
    }

    public class TumblrSubmissionWrapper : ISubmissionWrapper {
		public readonly PhotoPost Post;
        
		public TumblrSubmissionWrapper(PhotoPost post) {
            Post = post;
        }

        public string Title => "";
        public string HTMLDescription => Post.Caption;
        public bool Mature => false;
		public bool Adult => false;
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

	public class TumblrTextPostSubmissionWrapper : ISubmissionWrapper, IStatusUpdate {
		public readonly TextPost Post;

		public TumblrTextPostSubmissionWrapper(TextPost post) {
			Post = post;
		}

		public string Title => "";
		public string HTMLDescription => $"<h1>{Post.Title}</h1>{Post.Body}";
		public bool Mature => false;
		public bool Adult => false;
		public IEnumerable<string> Tags => Post.Tags;
		public DateTime Timestamp => Post.Timestamp;
		public string ViewURL => Post.Url;
		public string ImageURL => $"https://api.tumblr.com/v2/blog/{Post.BlogName}.tumblr.com/avatar/512";
		public string ThumbnailURL => $"https://api.tumblr.com/v2/blog/{Post.BlogName}.tumblr.com/avatar/128";
		public Color? BorderColor => null;

		public bool PotentiallySensitive => Mature || Adult;
		public bool HasPhoto => false;
		public IEnumerable<string> AdditionalLinks => Enumerable.Empty<string>();
	}

	public class DeletableTumblrSubmissionWrapper : TumblrSubmissionWrapper, IDeletable {
		private readonly TumblrClient _client;
		private readonly string _blogName;
		private readonly long _id;

		public DeletableTumblrSubmissionWrapper(TumblrClient client, PhotoPost post) : base(post) {
			_client = client;
			_blogName = post.BlogName;
			_id = post.Id;
		}

		public string SiteName => "Tumblr";

		public async Task DeleteAsync() {
			await _client.DeletePostAsync(_blogName, _id);
		}
	}
}
