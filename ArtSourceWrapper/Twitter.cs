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

    public class TwitterWrapper : IWrapper {
        private readonly ITwitterCredentials _credentials;

        private IUser _user;

        private UpdateGalleryParameters _lastUpdateGalleryParameters;
        private List<TwitterSubmissionWrapper> _cache;
        private int _currentOffset;

        public TwitterWrapper(ITwitterCredentials credentials) {
            _credentials = credentials;
            _cache = new List<TwitterSubmissionWrapper>();
        }

        public string SiteName => "Twitter";

        private Task<List<TwitterSubmissionWrapper>> GetSubmissions(int count) {
            return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
                List<TwitterSubmissionWrapper> list = _cache?.ToList() ?? new List<TwitterSubmissionWrapper>();
				int originalCount = list.Count;

                // Find the oldest tweet that exists in the already obtained page.
                long? minObtainedTweetId = list.Any()
					? list.Select(w => w.Tweet.Id).Min()
					: (long?)null;

                if (_user == null) {
                    _user = await UserAsync.GetAuthenticatedUser();
                    if (_user == null) throw new TwitterWrapperException("No user information returned from Twitter (rate limit reached or credentials no longer valid?)");
                }

                // Multiple calls may be needed (because retweets and tweets w/o photos are skipped).
				int tries = 0;
                while (list.Count < count) {
					if (tries++ > 10) {
						throw new TwitterWrapperException("No pictures were found in your recent tweets.");
					}

					var ps = new UserTimelineParameters {
                        ExcludeReplies = false,
                        IncludeEntities = true,
                        IncludeRTS = true,
                        MaximumNumberOfTweetsToRetrieve = 200
                    };
					ps.MaxId = (minObtainedTweetId ?? 0) - 1;

					// Get the tweets
					double newlyAdded = list.Count - originalCount;
					double needed = count - originalCount;
                    _lastUpdateGalleryParameters.Progress?.Report(newlyAdded / needed);
                    var tweets = await TimelineAsync.GetUserTimeline(_user, ps);

					// If no tweets were returned, then there are no more tweets
					if (!tweets.Any()) {
						break;
					}

                    // Wrap tweets
                    // Take no more than the size of the consumer's page (_lastUpdateGalleryParameters.Count)
                    // Skip retweets and tweets with no photos
                    foreach (var t in tweets.OrderByDescending(t => t.CreatedAt)) {
                        minObtainedTweetId = Math.Min(minObtainedTweetId ?? long.MaxValue, t.Id);

                        if (t.IsRetweet) continue;

						foreach (var m in t.Media) {
							if (m.MediaType == "photo") {
								list.Add(new TwitterSubmissionWrapper(t, m));
								tries = 0;
							}
						}
                    }
                }

                return _cache = list;
            });
        }

        public async Task<UpdateGalleryResult> NextPageAsync() {
            int count = _lastUpdateGalleryParameters.Count;
            _currentOffset += count;
            var list = await GetSubmissions(_currentOffset + count + 1);
            var list2 = new List<ISubmissionWrapper>();
            list2.AddRange(list.Skip(_currentOffset).Take(count));
            return new UpdateGalleryResult {
                HasLess = true,
                HasMore = list.Count > _currentOffset + count,
                Submissions = list2
            };
        }

        public async Task<UpdateGalleryResult> PreviousPageAsync() {
            int count = _lastUpdateGalleryParameters.Count;
            _currentOffset -= count;
            var list = await GetSubmissions(_currentOffset + count + 1);
            var list2 = new List<ISubmissionWrapper>();
            list2.AddRange(list.Skip(_currentOffset).Take(count));
            return new UpdateGalleryResult {
                HasLess = _currentOffset > 0,
                HasMore = list.Count > _currentOffset + count,
                Submissions = list2
            };
        }

        public async Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
            _lastUpdateGalleryParameters = p;
            _cache.Clear();
            _currentOffset = 0;
            var list = (await GetSubmissions(p.Count + 1))
                .Select(w => (ISubmissionWrapper)w)
                .ToList();
            return new UpdateGalleryResult {
                HasLess = false,
                HasMore = true,
                Submissions = list
            };
        }

        public Task<string> WhoamiAsync() {
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
