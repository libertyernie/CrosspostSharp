using DeviantartApi.Objects;
using DeviantartApi.Objects.SubObjects.DeviationMetadata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class DeviantArtException : Exception {
        public DeviantArtException(string message) : base(message) { }
    }

    public abstract class DeviantArtDeviationWrapper : AsynchronousCachedEnumerable<Deviation, uint> {
        private static string _clientId, _clientSecret;

        public static string ClientId {
            set {
                _clientId = value;
            }
        }

        public static string ClientSecret {
            set {
                _clientSecret = value;
            }
        }

        public string SiteName => "DeviantArt";
        public abstract string WrapperName { get; }

        public async Task<string> WhoamiAsync() {
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
        /// Uses the refresh token to "log in" and get a new set of tokens.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token (if any)</param>
        /// <returns>A new refresh token</returns>
        public static async Task<string> UpdateTokens(string refreshToken = null) {
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

    internal class DeviantArtScrapsUrlWrapper : AsynchronousCachedEnumerable<string, int> {
        public override int BatchSize { get; set; } = 24;
        public override int MinBatchSize => 24;
        public override int MaxBatchSize => 24;

        private readonly string _username;

        public DeviantArtScrapsUrlWrapper(string username) {
            _username = username;
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            int pos = startPosition ?? 0;
            
            var request = WebRequest.CreateHttp($"https://{_username}.deviantart.com/gallery/?catpath=scraps&offset={startPosition}");
            request.UserAgent = "ArtSync/3.0 (https://github.com/libertyernie/ArtSync)";
            var t1 = request.GetResponseAsync();
            await Task.WhenAny(t1, Task.Delay(5000));
            if (t1.Status == TaskStatus.WaitingForActivation) throw new DeviantArtException($"Data is taking too long to load rom {request.RequestUri} possibly due to an ArtSync bug. Try restarting your PC.");
            using (var response = await t1) {
                using (var sr = new StreamReader(response.GetResponseStream())) {
                    string html = await sr.ReadToEndAsync();

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    List<string> urls = new List<string>();

                    foreach (var link in doc.DocumentNode.Descendants("span")) {
                        var attribute = link.Attributes["data-deviationid"];
                        if (new[] { "data-deviationid", "href" }.All(s => link.Attributes.Select(a => a.Name).Contains(s))) {
                            urls.Add(link.Attributes["href"].Value);
                        }
                    }

                    return new InternalFetchResult(
                        urls,
                        pos + 24,
                        !doc.DocumentNode.Descendants("link").Any(n => n.Attributes["rel"].Value == "next"));
                }
            }
        }
    }

    public class DeviantArtScrapsDeviationWrapper : DeviantArtDeviationWrapper {
        private DeviantArtScrapsUrlWrapper _urlWrapper;

        public override string WrapperName => "DeviantArt (Public Scraps)";

        public override int BatchSize { get; set; } = 1;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 1;

        private static Regex APP_LINK = new Regex("DeviantArt://deviation/(........-....-....-....-............)");
        private static async Task<string> GetDeviationIdAsync(string url) {
            var request = WebRequest.CreateHttp(url);
            request.UserAgent = "ArtSync/3.0 (https://github.com/libertyernie/ArtSync)";
            var t1 = request.GetResponseAsync();
            await Task.WhenAny(t1, Task.Delay(5000));
            if (t1.Status == TaskStatus.WaitingForActivation) throw new DeviantArtException($"Data is taking too long to load rom {request.RequestUri}, possibly due to an ArtSync bug. Try restarting your PC.");
            using (var response = await t1) {
                using (var sr = new StreamReader(response.GetResponseStream())) {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null) {
                        var match = APP_LINK.Match(line);
                        if (match.Success) {
                            return match.Groups[1].Value;
                        }
                    }
                }
            }

            throw new DeviantArtException("Could not scrape GUID from DeviantArt page: " + url);
        }

        protected async override Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
            if (_urlWrapper == null) {
                _urlWrapper = new DeviantArtScrapsUrlWrapper(await WhoamiAsync());
            }

            uint skip = startPosition ?? 0;

            while (_urlWrapper.Cache.Count() < skip + 1 && !_urlWrapper.IsEnded) {
                await _urlWrapper.FetchAsync();
            }

            string url = _urlWrapper.Cache
                .Skip((int)skip)
                .FirstOrDefault();

            List<Deviation> dev = new List<Deviation>(1);

            if (url != null) {
                string id = await GetDeviationIdAsync(url);

                var response = await new DeviantartApi.Requests.Deviation.DeviationRequest(id).ExecuteAsync();

                if (response.IsError) {
                    throw new DeviantArtException(response.ErrorText);
                }
                if (!string.IsNullOrEmpty(response.Result.Error)) {
                    throw new DeviantArtException(response.Result.ErrorDescription);
                }

                dev.Add(response.Result);
            }

            return new InternalFetchResult(dev, skip + 1, !_urlWrapper.Cache.Skip((int)skip + 1).Any() && _urlWrapper.IsEnded);
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
