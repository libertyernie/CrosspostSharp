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

    public class TumblrWrapper : IWrapper {
        private readonly TumblrClient _client;
        private string _blogName;
		private IEnumerable<string> _blogNames;

        private UpdateGalleryParameters _lastUpdateGalleryParameters;
        private int _currentOffset;

        public TumblrWrapper(TumblrClient client, string blogName) {
            _client = client;
            _blogName = blogName;
        }

        public string SiteName => "Tumblr";

		private async Task<UpdateGalleryResult> GetPosts() {
			if (_currentOffset < 0) _currentOffset = 0;

			if (_blogNames == null) {
				var user = await _client.GetUserInfoAsync();
				_blogNames = user.Blogs.Select(b => b.Name).ToList();
			}
			if (!_blogNames.Contains(_blogName)) {
				throw new TumblrWrapperException($"The blog {_blogName} does not appear to be owned by the currently logged in user. (Make sure the name is spelled and capitalized correctly.)");
			}

			var posts = await _client.GetPostsAsync(
				_blogName,
				_currentOffset,
				_lastUpdateGalleryParameters.Count,
				type: PostType.Photo,
				includeReblogInfo: true);
			var list = posts.Result.Select(post => (PhotoPost)post).ToList();
			return new UpdateGalleryResult {
				HasLess = _currentOffset > 0,
				HasMore = list.Any(),
				Submissions = list.Select(post => (ISubmissionWrapper)new TumblrSubmissionWrapper(post, _blogNames.Contains(post.RebloggedRootName ?? post.BlogName))).ToList()
			};
		}

		public Task<UpdateGalleryResult> NextPageAsync() {
			_currentOffset += _lastUpdateGalleryParameters?.Count ?? 1;
			return GetPosts();
		}

        public Task<UpdateGalleryResult> PreviousPageAsync() {
			_currentOffset -= _lastUpdateGalleryParameters?.Count ?? 1;
			return GetPosts();
		}

        public Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
			_lastUpdateGalleryParameters = p;
			_currentOffset = 0;
			return GetPosts();
		}

        public async Task<string> WhoamiAsync() {
            var user = await _client.GetUserInfoAsync();
            return user.Name;
        }
    }

    public class TumblrSubmissionWrapper : ISubmissionWrapper {
		public readonly PhotoPost Post;

		private readonly bool _ownWork;
		public bool OwnWork => _ownWork;

		public TumblrSubmissionWrapper(PhotoPost post, bool ownWork) {
            Post = post;
			_ownWork = ownWork;
        }

        public string Title => "";
        public string HTMLDescription => Post.Caption;
        public bool PotentiallySensitive => false;
        public IEnumerable<string> Tags => Post.Tags;
        public string GeneratedUniqueTag => "#tumblr" + Post.Id;
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
        public Color? BorderColor => !OwnWork ? Color.Gray : (Color?)null;
	}
}
