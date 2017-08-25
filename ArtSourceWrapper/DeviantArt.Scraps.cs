using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
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

            string html = await DeviantartApi.Requester.MakeRequestRawAsync(new Uri($"https://{_username}.deviantart.com/gallery/?catpath=scraps&offset={startPosition}"));
            
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

    public class DeviantArtScrapsDeviationWrapper : DeviantArtDeviationWrapper {
        private DeviantArtScrapsUrlWrapper _urlWrapper;

        public override string WrapperName => "DeviantArt (Public Scraps)";

        public override int BatchSize { get; set; } = 1;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 1;

        private static Regex APP_LINK = new Regex("DeviantArt://deviation/(........-....-....-....-............)");
        private static async Task<string> GetDeviationIdAsync(string url) {
            string html = await DeviantartApi.Requester.MakeRequestRawAsync(new Uri(url));
            var match = APP_LINK.Match(html);
            if (match.Success) {
                return match.Groups[1].Value;
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
}
