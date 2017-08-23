using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    internal static class DeviantArtWebRequestHelper {
        public static async Task<WebResponse> GetResponseAsync(this WebRequest request, int delayMs) {
            var t1 = request.GetResponseAsync();
            await Task.WhenAny(t1, Task.Delay(delayMs));
            if (t1.Status == TaskStatus.WaitingForActivation) {
                throw new DeviantArtException($"Data is taking too long to load from {request.RequestUri}, possibly due to an ArtSync bug. Try restarting your PC.");
            } else {
                return await t1;
            }
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
            using (var response = await request.GetResponseAsync(5000)) {
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
            using (var response = await request.GetResponseAsync(5000)) {
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
}
