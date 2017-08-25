using FlickrNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class FlickrWrapper : SiteWrapper<FlickrSubmissionWrapper, uint> {
        private Flickr _flickr;

        public FlickrWrapper(string apiKey, string sharedSecret, string oAuthAccessToken, string oAuthAccessTokenSecret) {
            _flickr = new Flickr(apiKey, sharedSecret) {
                OAuthAccessToken = oAuthAccessToken,
                OAuthAccessTokenSecret = oAuthAccessTokenSecret
            };
        }

        public override string SiteName => "Flickr";
        public override string WrapperName => "Flickr";

        public override int BatchSize { get; set; } = 0;

        public override int MinBatchSize => 0;

        public override int MaxBatchSize => 0;

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

        protected async override Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
            return new InternalFetchResult(0, true);
        }
    }

    public class FlickrSubmissionWrapper : ISubmissionWrapper {
        public string Title => throw new NotImplementedException();

        public string HTMLDescription => throw new NotImplementedException();

        public bool PotentiallySensitive => throw new NotImplementedException();

        public IEnumerable<string> Tags => throw new NotImplementedException();

        public DateTime Timestamp => throw new NotImplementedException();

        public string ViewURL => throw new NotImplementedException();

        public string ImageURL => throw new NotImplementedException();

        public string ThumbnailURL => throw new NotImplementedException();

        public Color? BorderColor => throw new NotImplementedException();

        public bool OwnWork => throw new NotImplementedException();
    }
}
