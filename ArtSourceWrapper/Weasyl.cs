using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeasylLib;

namespace ArtSourceWrapper {
    public class WeasylWrapper : SiteWrapper<WeasylSubmissionWrapper, int> {
        protected WeasylClient _client;
        protected string _username;

        public override string SiteName => "Weasyl";

        public override int BatchSize { get; set; } = 100;
        public override int IndividualRequestsPerInvocation { get; set; } = 5;

        public WeasylWrapper(string apiKey) {
            _client = new WeasylClient(apiKey);
        }

        public override async Task<string> WhoamiAsync() {
            try {
                return (await _client.WhoamiAsync()).login;
            } catch (WebException e) when ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized) {
                throw new Exception("No username returned from Weasyl. The API key might be invalid or deleted.");
            }
        }

        /// <summary>
        /// Fetch submissions from Weasyl.
        /// </summary>
        /// <param name="startPosition">The nextid from the previous Weasyl search.</param>
        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition) {
            int maxCount = Math.Max(0, Math.Min(BatchSize, 100));

            if (_username == null) {
                _username = await WhoamiAsync();
            }

            var result = await _client.GetUserGalleryAsync(_username, nextid: startPosition, count: maxCount);
            
            var details = await Task.WhenAll(result.submissions.Take(IndividualRequestsPerInvocation).Select(s => _client.GetSubmissionAsync(s.submitid)));

            return new InternalFetchResult(
                details.OrderByDescending(d => d.posted_at).Select(d => new WeasylSubmissionWrapper(d)),
                result.nextid ?? 0,
                isEnded: result.nextid == null
            );
        }
    }

    public class WeasylCharacterWrapper : WeasylWrapper {
        public WeasylCharacterWrapper(string apiKey) : base(apiKey) { }

        public override string SiteName => "Weasyl (Characters)";

        /// <summary>
        /// Scrape the Weasyl site to load character IDs, and use the API to get information for each.
        /// </summary>
        /// <param name="startPosition">The ID of the lowest (oldest) character already downloaded.</param>
        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition) {
            int maxCount = Math.Max(0, BatchSize);

            if (_username == null) {
                _username = await WhoamiAsync();
            }
            List<int> all_ids = await _client.ScrapeCharacterIdsAsync(_username);

            IEnumerable<int> ids = all_ids
                .Where(id => id < (startPosition ?? int.MaxValue))
                .Take(maxCount);
            
            var details = await Task.WhenAll(ids.Take(IndividualRequestsPerInvocation).Select(i => _client.GetCharacterAsync(i)));

            return new InternalFetchResult(
                details.OrderByDescending(d => d.posted_at).Select(d => new WeasylSubmissionWrapper(d)),
                ids.DefaultIfEmpty(0).Min(),
                isEnded: !ids.Any()
            );
        }
    }

    public class WeasylSubmissionWrapper : ISubmissionWrapper {
        public string Rating => Submission.rating;
        public bool PotentiallySensitive => Rating != "general";

        public string GeneratedUniqueTag =>
            Submission is WeasylCharacterDetail ? $"#weasylcharacter{((WeasylCharacterDetail)Submission).charid}"
            : Submission is WeasylSubmissionDetail ? $"#weasyl{((WeasylSubmissionDetail)Submission).submitid}"
            : null;
        public string HTMLDescription => HtmlLinkUtils.MakeLinksAbsolute(Submission.HTMLDescription, "http://www.weasyl.com");
        public IEnumerable<string> Tags => Submission.tags;
        public DateTime Timestamp => Submission.posted_at;
        public string Title => Submission.title;

        public string ViewURL => Submission.link;
        public string ImageURL => Submission.media.submission.First().url;
        public string ThumbnailURL => Submission.media.thumbnail.First().url;

        public Color? BorderColor {
            get {
                switch (Submission.rating) {
                    case "mature":
                        return Color.FromArgb(170, 187, 34);
                    case "explicit":
                        return Color.FromArgb(185, 30, 35);
                    default:
                        return null;
                }
            }
		}

		public bool OwnWork => true;

		public WeasylSubmissionBaseDetail Submission { get; private set; }

        public WeasylSubmissionWrapper(WeasylSubmissionBaseDetail submission) {
            Submission = submission;
        }
    }
}
