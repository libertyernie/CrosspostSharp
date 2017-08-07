using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private readonly TwitterCredentials _credentials;

        private IUser _user;

        private UpdateGalleryParameters _lastUpdateGalleryParameters;
        private List<Task<UpdateGalleryResult>> _cache;
        private int _currentPage;

        public TwitterWrapper(TwitterCredentials credentials) {
            _credentials = credentials;
            _cache = new List<Task<UpdateGalleryResult>>();
        }

        public string SiteName => "Twitter";

        private Task<UpdateGalleryResult> CreatePage(int page) {
            return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
                // Find the oldest tweet that exists in the already obtained page.
                long minObtainedTweetId = 0;
                if (page > 0) {
                    var lastPage = await _cache[page - 1];
                    minObtainedTweetId = lastPage.Submissions.Select(w => (w as TwitterSubmissionWrapper)?.Tweet?.Id ?? 0).Min();
                }

                if (_user == null) {
                    _user = await UserAsync.GetAuthenticatedUser();
                    if (_user == null) throw new TwitterWrapperException("No user information returned from Twitter (rate limit reached or credentials no longer valid?)");
                }

                List<TwitterSubmissionWrapper> wrappers = new List<TwitterSubmissionWrapper>();
                bool hasMore = true;

                // Keep track of progress (1/2, 3/4, 7/8, ...)
                // Multiple calls may be needed (because retweets are skipped).
                float progress = 0;
                while (wrappers.Count < _lastUpdateGalleryParameters.Count) {
                    progress = (1 + progress) / 2;
                    _lastUpdateGalleryParameters.Progress?.Report(progress);

                    var ps = new UserTimelineParameters {
                        ExcludeReplies = false,
                        IncludeEntities = true,
                        IncludeRTS = true,
                        MaximumNumberOfTweetsToRetrieve = _lastUpdateGalleryParameters.Count * 2
                    };
                    ps.MaxId = minObtainedTweetId - 1;
                    var tweets = await TimelineAsync.GetUserTimeline(_user, ps);

                    // If no tweets were returned, then there are no more tweets
                    if (!tweets.Any()) {
                        hasMore = false;
                        break;
                    }

                    // Wrap tweets
                    // Take no more than the size of the consumer's page (_lastUpdateGalleryParameters.Count)
                    // Skip retweets
                    // Include tweets without photos (at least for now)
                    foreach (var t in tweets.OrderByDescending(t => t.CreatedAt)) {
                        if (t.IsRetweet) continue;

                        var firstPhoto = t.Media.Where(m => m.MediaType == "photo").FirstOrDefault();
                        wrappers.Add(new TwitterSubmissionWrapper(t, firstPhoto));

                        if (wrappers.Count == _lastUpdateGalleryParameters.Count) break;
                    }

                    minObtainedTweetId = wrappers.Select(w => (w as TwitterSubmissionWrapper)?.Tweet?.Id ?? 0).DefaultIfEmpty(minObtainedTweetId).Min();
                }

                // Progress is done
                _lastUpdateGalleryParameters.Progress?.Report(1);

                return new UpdateGalleryResult {
                    HasLess = page > 0,
                    HasMore = hasMore,
                    Submissions = wrappers.Select(w => (ISubmissionWrapper)w).ToList()
                };
            });
        }

        private Task<UpdateGalleryResult> GetPage(int page) {
            if (page < 0) page = 0;
            _currentPage = page;

            // Get all pages up to and including this one
            while (_cache.Count <= page) {
                _cache.Add(CreatePage(_cache.Count));
            }

            return _cache[page];
        }

        public Task<UpdateGalleryResult> NextPageAsync() {
            return GetPage(_currentPage + 1);
        }

        public Task<UpdateGalleryResult> PreviousPageAsync() {
            return GetPage(_currentPage - 1);
        }

        public Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
            _lastUpdateGalleryParameters = p;
            _cache.Clear();
            return GetPage(0);
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
        public string HTMLDescription => Tweet.FullText;
        public bool PotentiallySensitive => Tweet.PossiblySensitive;
        public IEnumerable<string> Tags => Tweet.Hashtags.Select(h => h.Text);
        public string GeneratedUniqueTag => "#tweet" + Tweet.IdStr;
        public DateTime Timestamp => Tweet.TweetLocalCreationDate;
        public string ViewURL => Tweet.Url;
        public string ImageURL => Media != null ? $"{Media.MediaURLHttps}:large" : Tweet.CreatedBy.ProfileImageUrl;
        public string ThumbnailURL => Media != null ? $"{Media.MediaURLHttps}:thumb" : Tweet.CreatedBy.ProfileImageUrl;
        public Color? BorderColor => Tweet.PossiblySensitive
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;
    }
}
