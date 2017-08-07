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
    public class TwitterWrapper : IWrapper {
        private TwitterCredentials _credentials;
        private long _lastMinId;
        private UpdateGalleryParameters _lastUpdateGalleryParameters;

        public TwitterWrapper(TwitterCredentials credentials) {
            _credentials = credentials;
        }

        public string SiteName => "Twitter";

        private Task<UpdateGalleryResult> UpdateGalleryInternalAsync() {
            return Auth.ExecuteOperationWithCredentials(_credentials, async () => {
                var user = await UserAsync.GetAuthenticatedUser();
                List<TwitterSubmissionWrapper> wrappers = new List<TwitterSubmissionWrapper>();
                _lastUpdateGalleryParameters.Progress?.Report(0.5f);
                var ps = new UserTimelineParameters {
                    ExcludeReplies = false,
                    IncludeEntities = true,
                    IncludeRTS = false,
                    MaximumNumberOfTweetsToRetrieve = _lastUpdateGalleryParameters.Count
                };
                ps.MaxId = _lastMinId - 1;
                var tweets = await TimelineAsync.GetUserTimeline(user, ps);
                foreach (var t in tweets) {
                    var firstPhoto = t.Media.Where(m => m.MediaType == "photo").FirstOrDefault();
                    wrappers.Add(new TwitterSubmissionWrapper(t, firstPhoto));
                }

                _lastUpdateGalleryParameters.Progress?.Report(1);

                _lastMinId = wrappers.Select(w => w.Tweet.Id).DefaultIfEmpty(0).Min();

                return new UpdateGalleryResult {
                    HasLess = false,
                    HasMore = wrappers.Any(),
                    Submissions = wrappers.Select(w => (ISubmissionWrapper)w).ToList()
                };
            });
        }

        public Task<UpdateGalleryResult> NextPageAsync() {
            return UpdateGalleryInternalAsync();
        }

        public Task<UpdateGalleryResult> PreviousPageAsync() {
            throw new NotSupportedException();
        }

        public Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
            _lastUpdateGalleryParameters = p;
            _lastMinId = 0;
            return UpdateGalleryInternalAsync();
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
        public string ImageURL {
            get {
                return ThumbnailURL;
            }
        }
        public string ThumbnailURL => Media?.MediaURLHttps ?? Tweet.CreatedBy?.ProfileImageUrl ?? "about:blank";
        public Color? BorderColor => Tweet.PossiblySensitive
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;
    }
}
