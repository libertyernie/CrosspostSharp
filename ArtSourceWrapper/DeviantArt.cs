using DeviantartApi.Objects;
using DeviantartApi.Objects.SubObjects.DeviationMetadata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class DeviantArtException : Exception {
        public DeviantArtException(string message) : base(message) { }
    }

    public static class DeviantArtLoginStatic {
        /// <summary>
        /// Uses the refresh token to "log in" and get a new set of tokens.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token (if any)</param>
        /// <returns>A new refresh token</returns>
        public static async Task<string> UpdateTokens(string clientId, string clientSecret, string refreshToken = null) {
            var result = await DeviantartApi.Login.SetAccessTokenByRefreshAsync(
                clientId,
                clientSecret,
                new Uri("https://www.example.com"),
                refreshToken ?? "",
                null,
                new[] { DeviantartApi.Login.Scope.Browse, DeviantartApi.Login.Scope.User, DeviantartApi.Login.Scope.Stash, DeviantartApi.Login.Scope.Publish });

            if (result.IsLoginError) {
                throw new DeviantArtException(result.LoginErrorText);
            }
            return result.RefreshToken;
        }
    }

    public abstract class DeviantArtDeviationWrapper : AsynchronousCachedEnumerable<Deviation, uint> {
        public string SiteName => "DeviantArt";
        public abstract string WrapperName { get; }

        public static async Task<User> GetUserAsync() {
            var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Result.Error)) {
                throw new DeviantArtException(result.Result.ErrorDescription);
            }
            return result.Result;
        }

        private User _user;

        public async Task<string> WhoamiAsync() {
            if (_user == null) _user = await GetUserAsync();
            return _user.Username;
        }

        public async Task<string> GetUserIconAsync() {
            if (_user == null) _user = await GetUserAsync();
            return _user.UserIconUrl.AbsoluteUri;
        }

        public static async Task<bool> LogoutAsync() {
            bool success = true;
            foreach (string token in new[] { DeviantartApi.Requester.AccessToken, DeviantartApi.Requester.RefreshToken }) {
                success = success && await DeviantartApi.Login.LogoutAsync(token);
            }
            return success;
        }
    }

    public class DeviantArtGalleryDeviationWrapper : DeviantArtDeviationWrapper {
        public override string WrapperName => "DeviantArt (Gallery)";
        public override int BatchSize { get; set; } = 24;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 24;

        protected override async Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
            uint maxCount = (uint)Math.Max(MinBatchSize, Math.Min(MaxBatchSize, count));
            uint position = startPosition ?? 0;

            var galleryResponse = await new DeviantartApi.Requests.Gallery.AllRequest() {
                Limit = maxCount,
                Offset = startPosition
            }.GetNextPageAsync();

            if (galleryResponse.IsError) {
                throw new DeviantArtException(galleryResponse.ErrorText);
            }
            if (!string.IsNullOrEmpty(galleryResponse.Result.Error)) {
                throw new DeviantArtException(galleryResponse.Result.ErrorDescription);
            }

            return new InternalFetchResult(
                galleryResponse.Result.Results,
                position + (uint)galleryResponse.Result.Results.Count,
                !galleryResponse.Result.HasMore);
        }
    }
    
    public class DeviantArtWrapper : SiteWrapper<DeviantArtSubmissionWrapper, uint> {
        private DeviantArtDeviationWrapper _idWrapper;

        public override string SiteName => _idWrapper.SiteName;
        public override string WrapperName => _idWrapper.WrapperName;

        public override int BatchSize { get; set; }
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => Math.Min(50, _idWrapper.MaxBatchSize);

        public DeviantArtWrapper(DeviantArtDeviationWrapper idWrapper) {
            _idWrapper = idWrapper;
            BatchSize = MaxBatchSize;
        }

        public override Task<string> WhoamiAsync() {
            return _idWrapper.WhoamiAsync();
        }

        public override Task<string> GetUserIconAsync(int size) {
            return _idWrapper.GetUserIconAsync();
        }

        private static IEnumerable<DeviantArtSubmissionWrapper> Wrap(IEnumerable<Deviation> deviations, IEnumerable<Metadata> metadata) {
            foreach (var d in deviations) {
                var metadata_if_any = metadata.FirstOrDefault(m => m.DeviationId == d.DeviationId);
                yield return new DeviantArtSubmissionWrapper(d, metadata_if_any);
            }
        }

        protected async override Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
            uint skip = startPosition ?? 0;
            int take = Math.Max(MinBatchSize, Math.Min(MaxBatchSize, BatchSize));

            while (_idWrapper.Cache.Count() < skip + take && !_idWrapper.IsEnded) {
                await _idWrapper.FetchAsync();
            }

            var deviations = _idWrapper.Cache
                .Skip(checked((int)skip))
                .Take(take);

            var metadataResponse = await new DeviantartApi.Requests.Deviation.MetadataRequest(deviations.Select(d => d.DeviationId)).ExecuteAsync();
            if (metadataResponse.IsError) {
                throw new DeviantArtException(metadataResponse.ErrorText);
            }
            if (!string.IsNullOrEmpty(metadataResponse.Result.Error)) {
                throw new DeviantArtException(metadataResponse.Result.ErrorDescription);
            }

            var wrappers = Wrap(deviations, metadataResponse.Result.Metadata);

            return new InternalFetchResult(wrappers, skip + (uint)take, !_idWrapper.Cache.Skip((int)skip + take).Any() && _idWrapper.IsEnded);
        }
    }

    public class DeviantArtSubmissionWrapper : ISubmissionWrapper {
        public bool PotentiallySensitive => Deviation.IsMature == true;
        
        public string HTMLDescription {
            get {
                string html = Metadata?.Description;
                if (html == null) return null;

                if (html.IndexOf("<p>", StringComparison.CurrentCultureIgnoreCase) == -1) {
                    html = $"<p>{html}</p>";
                }

                return html;
            }
        }
        public IEnumerable<string> Tags => Metadata?.Tags?.Select(t => t.TagName) ?? Enumerable.Empty<string>();
        public DateTime Timestamp => Deviation.PublishedTime ?? DateTime.Now;
        public string Title => Deviation.Title;

        public string ViewURL => Deviation.Url.AbsoluteUri;
        public string ImageURL => Deviation.Content.Src;
        public string ThumbnailURL => Deviation.Thumbs.FirstOrDefault()?.Src;

        public Color? BorderColor => Deviation.IsMature == true
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;
		
		public Deviation Deviation { get; private set; }
        public Metadata Metadata { get; private set; }

        public DeviantArtSubmissionWrapper(Deviation deviation, Metadata metadata = null) {
            if (deviation.DeviationId != metadata.DeviationId) throw new ArgumentException("DeviationId must be the same in both arguments");
            Deviation = deviation;
            Metadata = metadata;
        }
    }
}
