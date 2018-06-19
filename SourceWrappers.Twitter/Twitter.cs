using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace SourceWrappers.Twitter {
	public class TwitterPostWrapper : IPostWrapper, IStatusUpdate, IDeletable {
		public readonly ITweet _tweet;
		public readonly IMediaEntity _media;
		private ITwitterCredentials _credentials;

		public TwitterPostWrapper(ITweet tweet, IMediaEntity media, ITwitterCredentials credentials) {
			_tweet = tweet;
			_media = media;
			_credentials = credentials;
		}

		public string Title => "";
		public string HTMLDescription {
			get {
				string text = _media == null
					? _tweet.FullText
					: _tweet.FullText.Replace(_media.URL, "");
				return "<p>" + WebUtility.HtmlEncode(text).Replace("\n", "<br/>") + "</p>";
			}
		}
		public bool Mature => _tweet.PossiblySensitive;
		public bool Adult => false;
		public IEnumerable<string> Tags => _tweet.Hashtags.Select(h => h.Text);
		public DateTime Timestamp => _tweet.CreatedAt;
		public string ViewURL => _tweet.Url;
		public string ImageURL => _media != null ? $"{_media.MediaURLHttps}:large" : _tweet.CreatedBy.ProfileImageUrl;
		public string ThumbnailURL => _media != null ? $"{_media.MediaURLHttps}:thumb" : _tweet.CreatedBy.ProfileImageUrl;

		public string SiteName => "Twitter";

		public string FullHTML => HTMLDescription;
		public bool PotentiallySensitive => Mature || Adult;
		public bool HasPhoto => _media != null;
		public IEnumerable<string> AdditionalLinks => _tweet.Entities.Urls.Select(u => u.ExpandedURL);

		public Task DeleteAsync() {
			return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
				await TweetAsync.DestroyTweet(_tweet);
			});
		}
	}

	public class TwitterSourceWrapper : IPagedSourceWrapper<long> {
		private readonly ITwitterCredentials _credentials;
		private readonly bool _photosOnly;

		public TwitterSourceWrapper(ITwitterCredentials credentials, bool photosOnly = true) {
			_credentials = credentials;
			_photosOnly = photosOnly;
		}

		private IUser _user;

		public async Task<IUser> GetUserAsync() {
			if (_user == null) {
				_user = await Auth.ExecuteOperationWithCredentials(_credentials, () => UserAsync.GetAuthenticatedUser());
				if (_user == null) throw new Exception("No user information returned from Twitter (rate limit reached or credentials no longer valid?)");
			}
			return _user;
		}

		public string Name => _photosOnly ? "Twitter (photos)" : "Twitter (text + photos)";
		public int SuggestedBatchSize => 20;

		public Task<FetchResult<long>> MoreAsync(long cursor, int take) {
			return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
				var ps = new UserTimelineParameters {
					ExcludeReplies = false,
					IncludeEntities = true,
					IncludeRTS = true,
					MaxId = cursor,
					MaximumNumberOfTweetsToRetrieve = take
				};
				
				var tweets = await TimelineAsync.GetUserTimeline(await GetUserAsync(), ps);

				IEnumerable<IPostWrapper> Wrap() {
					foreach (var t in tweets.OrderByDescending(t => t.CreatedAt)) {
						if (t.IsRetweet) continue;

						foreach (var m in t.Media) {
							if (m.MediaType == "photo") {
								yield return new TwitterPostWrapper(t, m, _credentials);
							}
						}

						if (!_photosOnly && !t.Media.Any(m => m.MediaType == "photo")) {
							yield return new TwitterPostWrapper(t, null, _credentials);
						}
					}
				}

				return new FetchResult<long>(
					posts: Wrap(),
					next: tweets.Select(t => t.Id).Min() - 1,
					hasMore: tweets.Any());
			});
		}

		public Task<FetchResult<long>> StartAsync(int take) {
			return MoreAsync(-1, take);
		}

		public async Task<IEnumerable<IPostWrapper>> FetchAllAsync(int limit) {
			var list = new List<IPostWrapper>();

			var result = await StartAsync(limit);
			list.AddRange(result.Posts);

			while (result.HasMore && list.Count < limit) {
				result = await MoreAsync(result.Next, limit - list.Count);
				list.AddRange(result.Posts);
			}

			return list;
		}

		public async Task<string> WhoamiAsync() {
			return (await GetUserAsync()).ScreenName;
		}

		public async Task<string> GetUserIconAsync(int size) {
			return (await GetUserAsync()).ProfileImageUrl;
		}
	}
}
