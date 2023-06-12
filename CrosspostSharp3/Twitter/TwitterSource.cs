using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace CrosspostSharp3.Twitter {
	public class TwitterSource : IArtworkSource {
		private readonly ITwitterClient _client;

		public TwitterSource(ITwitterClient client) {
			_client = client;
		}

		public string Name => "Twitter";

		private class TwitterPostWrapper : IPostBase {
			private readonly ITweet _tweet;

			public TwitterPostWrapper(ITweet tweet) {
				_tweet = tweet;
			}

			public string Title => "";

			public string HTMLDescription {
				get {
					string str = _tweet.FullText;
					foreach (var m in _tweet.Media)
						str = str.Replace(m.URL, "");
					return str;
				}
			}

			public bool Mature => _tweet.PossiblySensitive;
			public bool Adult => _tweet.PossiblySensitive;
			public IEnumerable<string> Tags => _tweet.Hashtags.Select(x => x.Text);
			public DateTime Timestamp => _tweet.CreatedAt.UtcDateTime;
			public string ViewURL => _tweet.Url;
		}

		private class TwitterPhotoPostWrapper : TwitterPostWrapper, IRemotePhotoPost {
			private readonly IMediaEntity _media;

			public TwitterPhotoPostWrapper(ITweet tweet, IMediaEntity media) : base(tweet) {
				_media = media;
			}

			public string ImageURL => _media.MediaURLHttps + ":large";
			public string ThumbnailURL => _media.MediaURLHttps + ":thumb";
		}

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();

			var ps = new GetUserTimelineParameters(user.Name) {
				ExcludeReplies = false,
				IncludeEntities = true,
				IncludeRetweets = false
			};

			while (true) {
				var tweets = await _client.Timelines.GetUserTimelineAsync(ps);
				foreach (var t in tweets) {
					var photos = t.Media.Where(m => m.MediaType == "photo");
					foreach (var m in photos)
						yield return new TwitterPhotoPostWrapper(t, m);

					if (!photos.Any())
						yield return new TwitterPostWrapper(t);
				}

				if (!tweets.Any())
					break;

				ps.MaxId = tweets.Select(x => x.Id).Min() - 1;
			}
		}

		private record Author : IAuthor {
			public string Name { get; init; }
			public string IconUrl { get; init; }
		}

		public async Task<IAuthor> GetUserAsync() {
			var user = await _client.Users.GetAuthenticatedUserAsync();
			return new Author {
				Name = user.ScreenName,
				IconUrl = user.ProfileImageUrl
			};
		}
	}
}
