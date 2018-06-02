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
    public abstract class TumblrSubmissionWrapper<T> : ISubmissionWrapper, IDeletable where T : BasePost {
		private readonly TumblrClient _client;
		public readonly T Post;
		
		public TumblrSubmissionWrapper(TumblrClient client, T post) {
			_client = client;
			Post = post ?? throw new ArgumentNullException(nameof(post));
		}

		public virtual string Title => "";
		public abstract string HTMLDescription { get; }
		public bool Mature => false;
		public bool Adult => false;
		public IEnumerable<string> Tags => Post.Tags;
		public DateTime Timestamp => Post.Timestamp;
		public string ViewURL => Post.Url;
		public virtual string ImageURL => $"https://api.tumblr.com/v2/blog/{Post.BlogName}.tumblr.com/avatar/512";
		public virtual string ThumbnailURL => $"https://api.tumblr.com/v2/blog/{Post.BlogName}.tumblr.com/avatar/128";
		public Color? BorderColor => null;
		
		public string SiteName => "Tumblr";

		public async Task DeleteAsync() {
			await _client.DeletePostAsync(Post.BlogName, Post.Id);
		}
	}

    public class TumblrPhotoPostSubmissionWrapper : TumblrSubmissionWrapper<PhotoPost> {
		public TumblrPhotoPostSubmissionWrapper(TumblrClient client, PhotoPost post) : base(client, post) { }
		
        public override string HTMLDescription => Post.Caption;
        public override string ImageURL => Post.Photo.OriginalSize.ImageUrl;
        public override string ThumbnailURL {
            get {
				foreach (var alt in Post.Photo.AlternateSizes.OrderBy(s => s.Width)) {
                    if (alt.Width < 120 && alt.Height < 120) continue;
                    return alt.ImageUrl;
                }
                return Post.Photo.OriginalSize.ImageUrl;
            }
        }
	}

	public class TumblrTextPostSubmissionWrapper : TumblrSubmissionWrapper<TextPost>, IStatusUpdate {
		public TumblrTextPostSubmissionWrapper(TumblrClient client, TextPost post) : base(client, post) { }

		public override string Title => Post.Title;
		public override string HTMLDescription => Post.Body;

		public string FullHTML => string.IsNullOrEmpty(Title)
			? HTMLDescription
			: $"<h1>{Title}</h1>{HTMLDescription}";
		public bool PotentiallySensitive => Mature || Adult;
		public bool HasPhoto => false;
		public IEnumerable<string> AdditionalLinks => Enumerable.Empty<string>();
	}
}
