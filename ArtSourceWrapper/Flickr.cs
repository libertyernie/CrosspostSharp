using FlickrNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class FlickrWrapper : SiteWrapper<FlickrSubmissionWrapper, int> {
        private Flickr _flickr;

        public FlickrWrapper(string apiKey, string sharedSecret, string oAuthAccessToken, string oAuthAccessTokenSecret) {
            _flickr = new Flickr(apiKey, sharedSecret) {
                OAuthAccessToken = oAuthAccessToken,
                OAuthAccessTokenSecret = oAuthAccessTokenSecret
            };
        }

        public FlickrWrapper(Flickr client) {
            _flickr = client;
        }

        public override string SiteName => "Flickr";
        public override string WrapperName => "Flickr";

        public override int BatchSize { get; set; } = 100;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 500;

        private FoundUser _cachedUser;

        public async Task<FoundUser> GetUserAsync() {
            if (_cachedUser == null) {
                var t = new TaskCompletionSource<Auth>();
                _flickr.AuthOAuthCheckTokenAsync(result => {
                    if (result.HasError) {
                        t.SetException(result.Error);
                    } else {
                        t.SetResult(result.Result);
                    }
                });
                var oauth = await t.Task;
                _cachedUser = oauth.User;
            }
            return _cachedUser;
        }
        
        public async override Task<string> WhoamiAsync() {
            return (await GetUserAsync()).UserName;
        }

        public async override Task<string> GetUserIconAsync(int size) {
            string id = (await GetUserAsync()).UserId;

            var t = new TaskCompletionSource<Person>();
            _flickr.PeopleGetInfoAsync(id, result => {
                if (result.HasError) {
                    t.SetException(result.Error);
                } else {
                    t.SetResult(result.Result);
                }
            });
            var person = await t.Task;

            return person.IconServer != "0"
                ? $"http://farm{person.IconFarm}.staticflickr.com/{person.IconServer}/buddyicons/{person.UserId}.jpg"
                : $"https://www.flickr.com/images/buddyicon.gif";
        }

        protected async override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            var t = new TaskCompletionSource<PhotoCollection>();
            _flickr.PeopleGetPhotosAsync("me",
                PhotoSearchExtras.Description | PhotoSearchExtras.Tags | PhotoSearchExtras.DateUploaded | PhotoSearchExtras.OriginalFormat,
                startPosition ?? 1,
                count,
                result => {
                    if (result.HasError) {
                        t.SetException(result.Error);
                    } else {
                        t.SetResult(result.Result);
                    }
                });
            var r = await t.Task;

            return new InternalFetchResult(r.Select(p => new FlickrSubmissionWrapper(p)), r.Page + 1, r.Page == r.Pages);
        }
    }

    public class FlickrSubmissionWrapper : ISubmissionWrapper {
        private Photo _photo;
        public FlickrSubmissionWrapper(Photo photo) {
            _photo = photo;
        }

        public string Title => _photo.Title;
        public string HTMLDescription => _photo.Description;
        public bool Mature => false;
        public bool Adult => false;
		public IEnumerable<string> Tags => _photo.Tags;
        public DateTime Timestamp => _photo.DateUploaded;
        public string ViewURL => $"https://www.flickr.com/photos/{_photo.UserId}/{_photo.PhotoId}";
        public string ImageURL => $"https://farm{_photo.Farm}.staticflickr.com/{_photo.Server}/{_photo.PhotoId}_{_photo.OriginalSecret}_o.{_photo.OriginalFormat}";
        public string ThumbnailURL => $"https://farm{_photo.Farm}.staticflickr.com/{_photo.Server}/{_photo.PhotoId}_{_photo.Secret}_q.jpg";
        public Color? BorderColor => null;
    }
}
