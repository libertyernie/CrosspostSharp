using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace ArtSourceWrapper {
    public class TwitterWrapperException : Exception {
        public TwitterWrapperException(string message) : base(message) { }
    }

    public class TwitterWrapper : SiteWrapper<TwitterSubmissionWrapper, long> {
        private readonly ITwitterCredentials _credentials;
		private readonly bool _photosOnly;

		public override string WrapperName => _photosOnly ? "Twitter (photos only)" : "Twitter (all)";
		public override bool SubmissionsFiltered => _photosOnly;

		public override int BatchSize { get; set; } = 200;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 200;

        public TwitterWrapper(ITwitterCredentials credentials, bool photosOnly = true) {
            _credentials = credentials;
			_photosOnly = photosOnly;

		}

        private IEnumerable<TwitterSubmissionWrapper> Wrap(IEnumerable<ITweet> tweets) {
            foreach (var t in tweets.OrderByDescending(t => t.CreatedAt)) {
                if (t.IsRetweet) continue;

                foreach (var m in t.Media) {
                    if (m.MediaType == "photo") {
                        yield return new TwitterSubmissionWrapper(t, m, _credentials);
                    }
                }

				if (!_photosOnly && !t.Media.Any(m => m.MediaType == "photo")) {
					yield return new TwitterSubmissionWrapper(t, null, _credentials);
				}
            }
        }

        private IUser _user;

        public async Task<IUser> GetUserAsync() {
            if (_user == null) {
                _user = await Auth.ExecuteOperationWithCredentials(_credentials, () => UserAsync.GetAuthenticatedUser());
                if (_user == null) throw new TwitterWrapperException("No user information returned from Twitter (rate limit reached or credentials no longer valid?)");
            }
            return _user;
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(long? startPosition, int maxCount) {
            return await Auth.ExecuteOperationWithCredentials(_credentials, async () => {
                var ps = new UserTimelineParameters {
                    ExcludeReplies = false,
                    IncludeEntities = true,
                    IncludeRTS = true,
                    MaximumNumberOfTweetsToRetrieve = maxCount
                };
                ps.MaxId = startPosition ?? -1;

                // Get the tweets
                var tweets = await TimelineAsync.GetUserTimeline(await GetUserAsync(), ps);

                // If no tweets were returned, then there are no more tweets
                if (!tweets.Any()) {
                    return new InternalFetchResult(ps.MaxId, isEnded: true);
                }

                return new InternalFetchResult(Wrap(tweets), tweets.Select(t => t.Id).Min() - 1);
            });
        }

        public override async Task<string> WhoamiAsync() {
            return (await GetUserAsync()).ScreenName;
        }

        public override async Task<string> GetUserIconAsync(int size) {
            return (await GetUserAsync()).ProfileImageUrl;
        }
    }

    public class TwitterSubmissionWrapper : ISubmissionWrapper, IStatusUpdate, IDeletable {
        public readonly ITweet Tweet;
        public readonly IMediaEntity Media;

		private ITwitterCredentials _credentials;

        public TwitterSubmissionWrapper(ITweet tweet, IMediaEntity media, ITwitterCredentials credentials) {
            Tweet = tweet;
            Media = media;
			_credentials = credentials;
		}

        public string Title => "";
        public string HTMLDescription {
            get {
                string text = Media == null
					? Tweet.FullText
					: Tweet.FullText.Replace(Media.URL, "");
                return "<p>" + WebUtility.HtmlEncode(text).Replace("\n", "<br/>") + "</p>";
            }
        }
        public bool Mature => Tweet.PossiblySensitive;
		public bool Adult => false;
        public IEnumerable<string> Tags => Tweet.Hashtags.Select(h => h.Text);
        public DateTime Timestamp => Tweet.CreatedAt;
        public string ViewURL => Tweet.Url;
        public string ImageURL => Media != null ? $"{Media.MediaURLHttps}:large" : Tweet.CreatedBy.ProfileImageUrl;
        public string ThumbnailURL => Media != null ? $"{Media.MediaURLHttps}:thumb" : Tweet.CreatedBy.ProfileImageUrl;
        public Color? BorderColor => Tweet.PossiblySensitive
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;
		
		public string SiteName => "Twitter";

		public string FullHTML => HTMLDescription;
		public bool PotentiallySensitive => Mature || Adult;
		public bool HasPhoto => Media != null;
		public IEnumerable<string> AdditionalLinks => Tweet.Entities.Urls.Select(u => u.ExpandedURL);

		public Task DeleteAsync() {
			return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
				await TweetAsync.DestroyTweet(Tweet);
			});
		}
	}
}
