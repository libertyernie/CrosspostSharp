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

    public class DeviantArtWrapper : SiteWrapper<DeviantArtSubmissionWrapper, uint> {
        private static string _clientId, _clientSecret;
        private static bool _initialLogin;

        public static string ClientId {
            set {
                _clientId = value;
                _initialLogin = false;
            }
        }

        public static string ClientSecret {
            set {
                _clientSecret = value;
                _initialLogin = false;
            }
        }

        public static async Task<string> WhoamiStaticAsync() {
            if (!_initialLogin) await UpdateTokens();

            var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Result.Error)) {
                throw new DeviantArtException(result.Result.ErrorDescription);
            }
            return result.Result.Username;
        }

        /// <summary>
        /// Uses the refresh token to "log in" and get a new set of tokens. If the refresh token is null, invalid, or not provided, a login window will be shown.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token (if any)</param>
        /// <returns>A new refresh token</returns>
        public static async Task<string> UpdateTokens(string refreshToken = null) {
            _initialLogin = true;
            var result = await DeviantartApi.Login.SetAccessTokenByRefreshAsync(
                _clientId,
                _clientSecret,
                new Uri("https://www.example.com"),
                refreshToken ?? "",
                null,
                new[] { DeviantartApi.Login.Scope.Browse, DeviantartApi.Login.Scope.User, DeviantartApi.Login.Scope.Stash, DeviantartApi.Login.Scope.Publish });
            if (result.IsLoginError) {
                throw new DeviantArtException(result.LoginErrorText);
            }
            return result.RefreshToken;
        }

        public override string SiteName => "DeviantArt";

        public override Task<string> WhoamiAsync() {
            return WhoamiStaticAsync();
        }

        private IEnumerable<DeviantArtSubmissionWrapper> Wrap(IEnumerable<Deviation> deviations, IEnumerable<Metadata> metadata) {
            foreach (var d in deviations) {
                var metadata_if_any = metadata.FirstOrDefault(m => m.DeviationId == d.DeviationId);
                yield return new DeviantArtSubmissionWrapper(d, metadata_if_any);
            }
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, ushort? maxCount) {
            if (maxCount > 24) maxCount = 24;
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

            if (!galleryResponse.Result.HasMore) {
                return new InternalFetchResult(position, true);
            }

            var metadataResponse = await new DeviantartApi.Requests.Deviation.MetadataRequest(galleryResponse.Result.Results.Select(d => d.DeviationId)).ExecuteAsync();
            if (metadataResponse.IsError) {
                throw new DeviantArtException(metadataResponse.ErrorText);
            }
            if (!string.IsNullOrEmpty(metadataResponse.Result.Error)) {
                throw new DeviantArtException(metadataResponse.Result.ErrorDescription);
            }

            return new InternalFetchResult(
                Wrap(galleryResponse.Result.Results, metadataResponse.Result.Metadata),
                position + (uint)galleryResponse.Result.Results.Count);
        }

        public static async Task<bool> LogoutAsync() {
            bool success = true;
            foreach (string token in new[] { DeviantartApi.Requester.AccessToken, DeviantartApi.Requester.RefreshToken }) {
                success = success && await DeviantartApi.Login.LogoutAsync(token);
            }
            return success;
        }
    }

    public class DeviantArtSubmissionWrapper : ISubmissionWrapper {
        public bool PotentiallySensitive => Deviation.IsMature == true;

        public string GeneratedUniqueTag => $"#deviantart-{Deviation.DeviationId}";
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

		public bool OwnWork => true;

		public Deviation Deviation { get; private set; }
        public Metadata Metadata { get; private set; }

        public DeviantArtSubmissionWrapper(Deviation deviation, Metadata metadata = null) {
            if (deviation.DeviationId != metadata.DeviationId) throw new ArgumentException("DeviationId must be the same in both arguments");
            Deviation = deviation;
            Metadata = metadata;
        }
    }
}
