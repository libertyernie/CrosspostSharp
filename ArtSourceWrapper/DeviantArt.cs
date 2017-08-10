using DeviantartApi.Objects;
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
        private string _clientId, _clientSecret;
        private bool _initialLogin;

        public override string SiteName => "DeviantArt";

        public DeviantArtWrapper(string clientId, string clientSecret) {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _initialLogin = false;
        }

        /// <summary>
        /// Uses the refresh token to "log in" and get a new set of tokens. If the refresh token is null, invalid, or not provided, a login window will be shown.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token (if any)</param>
        /// <returns>A new refresh token</returns>
        public async Task<string> UpdateTokens(string refreshToken = null) {
            _initialLogin = true;
            var result = await DeviantartApi.Login.SetAccessTokenByRefreshAsync(
                _clientId,
                _clientSecret,
                "https://www.example.com",
                refreshToken ?? "",
                null,
                new[] { DeviantartApi.Login.Scope.Browse, DeviantartApi.Login.Scope.User });
            if (result.IsLoginError) {
                throw new DeviantArtException(result.LoginErrorText);
            }
            return result.RefreshToken;
        }

        public override async Task<string> WhoamiAsync() {
            if (!_initialLogin) await UpdateTokens();

            var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Object.Error)) {
                throw new DeviantArtException(result.Object.ErrorDescription);
            }
            return result.Object.Username;
        }

        private IEnumerable<DeviantArtSubmissionWrapper> Wrap(IEnumerable<Deviation> deviations, IEnumerable<DeviationMetadata.MetadataClass> metadata) {
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
            if (!string.IsNullOrEmpty(galleryResponse.Object.Error)) {
                throw new DeviantArtException(galleryResponse.Object.ErrorDescription);
            }

            if (!galleryResponse.Object.HasMore) {
                return new InternalFetchResult(position, true);
            }

            var metadataResponse = await new DeviantartApi.Requests.Deviation.MetadataRequest() {
                DeviationIds = new HashSet<string>(galleryResponse.Object.Results.Select(d => d.DeviationId))
            }.ExecuteAsync();
            if (metadataResponse.IsError) {
                throw new DeviantArtException(metadataResponse.ErrorText);
            }
            if (!string.IsNullOrEmpty(metadataResponse.Object.Error)) {
                throw new DeviantArtException(metadataResponse.Object.ErrorDescription);
            }

            return new InternalFetchResult(
                Wrap(galleryResponse.Object.Results, metadataResponse.Object.Metadata),
                position + (uint)galleryResponse.Object.Results.Count);
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
        public bool PotentiallySensitive => Deviation.IsMature;

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
        public DateTime Timestamp => Deviation.PublishedTime;
        public string Title => Deviation.Title;

        public string ViewURL => Deviation.Url;
        public string ImageURL => Deviation.Content.Src;
        public string ThumbnailURL => Deviation.Thumbs.FirstOrDefault()?.Src;

        public Color? BorderColor => Deviation.IsMature
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;

		public bool OwnWork => true;

		public Deviation Deviation { get; private set; }
        public DeviationMetadata.MetadataClass Metadata { get; private set; }

        public DeviantArtSubmissionWrapper(Deviation deviation, DeviationMetadata.MetadataClass metadata = null) {
            if (deviation.DeviationId != metadata.DeviationId) throw new ArgumentException("DeviationId must be the same in both arguments");
            Deviation = deviation;
            Metadata = metadata;
        }
    }
}
