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
        private IUser _user;

        public override string SiteName => "Twitter";

        public override int BatchSize { get; set; } = 200;
        public override int IndividualRequestsPerInvocation { get; set; } = 0;

        public TwitterWrapper(ITwitterCredentials credentials) {
            _credentials = credentials;
        }

        private static IEnumerable<TwitterSubmissionWrapper> Wrap(IEnumerable<ITweet> tweets) {
            foreach (var t in tweets.OrderByDescending(t => t.CreatedAt)) {
                if (t.IsRetweet) continue;

                foreach (var m in t.Media) {
                    if (m.MediaType == "photo") {
                        yield return new TwitterSubmissionWrapper(t, m);
                    }
                }
            }
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(long? startPosition) {
            int maxCount = Math.Max(0, Math.Min(BatchSize, 20));

            return await Auth.ExecuteOperationWithCredentials(_credentials, async () => {
                if (_user == null) {
                    _user = await UserAsync.GetAuthenticatedUser();
                    if (_user == null) throw new TwitterWrapperException("No user information returned from Twitter (rate limit reached or credentials no longer valid?)");
                }
                
                var ps = new UserTimelineParameters {
                    ExcludeReplies = false,
                    IncludeEntities = true,
                    IncludeRTS = true,
                    MaximumNumberOfTweetsToRetrieve = maxCount
                };
                ps.MaxId = startPosition ?? -1;

                // Get the tweets
                var tweets = await TimelineAsync.GetUserTimeline(_user, ps);

                // If no tweets were returned, then there are no more tweets
                if (!tweets.Any()) {
                    return new InternalFetchResult(ps.MaxId, isEnded: true);
                }

                return new InternalFetchResult(Wrap(tweets), tweets.Select(t => t.Id).Min() - 1);
            });
        }

        public override Task<string> WhoamiAsync() {
            return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
                var user = await UserAsync.GetAuthenticatedUser();
                return user?.ScreenName;
            });
        }
    }

    public class TwitterSubmissionWrapper : ISubmissionWrapper {
        public readonly ITweet Tweet;
        public readonly IMediaEntity Media;

        public TwitterSubmissionWrapper(ITweet tweet, IMediaEntity media) {
            Tweet = tweet;
            Media = media;
        }

        public string Title => "";
        public string HTMLDescription {
            get {
                string text = Tweet.FullText.Replace(Media.URL, "");
                return "<p>" + WebUtility.HtmlEncode(text).Replace("\n", "<br/>") + "</p>";
            }
        }
        public bool PotentiallySensitive => Tweet.PossiblySensitive;
        public IEnumerable<string> Tags => Tweet.Hashtags.Select(h => h.Text);
        public string GeneratedUniqueTag => "#tweet" + Tweet.IdStr;
        public DateTime Timestamp => Tweet.CreatedAt;
        public string ViewURL => Tweet.Url;
        public string ImageURL => Media != null ? $"{Media.MediaURLHttps}:large" : Tweet.CreatedBy.ProfileImageUrl;
        public string ThumbnailURL => Media != null ? $"{Media.MediaURLHttps}:thumb" : Tweet.CreatedBy.ProfileImageUrl;
        public Color? BorderColor => Tweet.PossiblySensitive
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;

		public bool OwnWork => !Tweet.IsRetweet;
	}
}
