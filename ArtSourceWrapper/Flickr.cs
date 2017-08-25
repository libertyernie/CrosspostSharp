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

        public override string SiteName => "Flickr";
        public override string WrapperName => "Flickr";

        public override int BatchSize { get; set; } = 100;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 500;

        private Task<Auth> AuthOAuthCheckTokenAsync() {
            var t = new TaskCompletionSource<Auth>();
            _flickr.AuthOAuthCheckTokenAsync(a => {
                if (a.HasError) {
                    t.SetException(a.Error);
                } else {
                    t.SetResult(a.Result);
                }
            });
            return t.Task;
        }

        public async override Task<string> WhoamiAsync() {
            var oauth = await AuthOAuthCheckTokenAsync();
            return oauth.User.UserName;
        }

        private Task<PhotoCollection> PeopleGetPhotosAsync(string userId, PhotoSearchExtras extras, int page, int perPage) {
            var t = new TaskCompletionSource<PhotoCollection>();
            _flickr.PeopleGetPhotosAsync(userId, extras, page, perPage, a => {
                if (a.HasError) {
                    t.SetException(a.Error);
                } else {
                    t.SetResult(a.Result);
                }
            });
            return t.Task;
        }

        protected async override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            var r = await PeopleGetPhotosAsync("me",
                PhotoSearchExtras.Description | PhotoSearchExtras.Tags | PhotoSearchExtras.DateUploaded | PhotoSearchExtras.OriginalFormat,
                startPosition ?? 1,
                count);

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
        public bool PotentiallySensitive => false;
        public IEnumerable<string> Tags => _photo.Tags;
        public DateTime Timestamp => _photo.DateUploaded;
        public string ViewURL => $"https://www.flickr.com/photos/{_photo.UserId}/{_photo.PhotoId}";
        public string ImageURL => $"https://farm{_photo.Farm}.staticflickr.com/{_photo.Server}/{_photo.PhotoId}_{_photo.OriginalSecret}_o.{_photo.OriginalFormat}";
        public string ThumbnailURL => $"https://farm{_photo.Farm}.staticflickr.com/{_photo.Server}/{_photo.PhotoId}_{_photo.Secret}_q.jpg";
        public Color? BorderColor => null;
        public bool OwnWork => true;
    }
}
