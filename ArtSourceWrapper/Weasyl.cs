using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeasylSyncLib;

namespace ArtSourceWrapper {
    public class WeasylWrapper : SiteWrapper<WeasylSubmissionWrapper, int> {
        protected WeasylAPI _client;
        protected string _username;

        public override string SiteName => "Weasyl";

        public WeasylWrapper(string apiKey) {
            _client = new WeasylAPI { APIKey = apiKey };
        }

        public override async Task<string> WhoamiAsync() {
            return (await _client.Whoami())?.login;
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, ushort? maxCount) {
            if (maxCount > 100) maxCount = 100;

            if (_username == null) {
                _username = await WhoamiAsync();
            }

            var result = await _client.UserGallery(_username, nextid: startPosition, count: maxCount);

            DateTime dt = DateTime.Now;
            var detailTasks = new List<Task<SubmissionBaseDetail>>(result.submissions.Length);
            var details = new List<SubmissionBaseDetail>(result.submissions.Length);
            foreach (var s in result.submissions) {
                detailTasks.Add(_client.ViewSubmission(s.submitid));
            }
            foreach (var task in detailTasks) {
                details.Add(await task);
            }
            System.Diagnostics.Debug.WriteLine(DateTime.Now - dt);

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

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, ushort? maxCount) {
            List<int> all_ids = await _client.GetCharacterIds(_username);

            IEnumerable<int> ids = all_ids
                .Where(id => id < (startPosition ?? int.MaxValue))
                .Take(maxCount ?? int.MaxValue);

            DateTime dt = DateTime.Now;
            var detailTasks = new List<Task<SubmissionBaseDetail>>(ids.Count());
            var details = new List<SubmissionBaseDetail>(ids.Count());
            foreach (int id in ids) {
                detailTasks.Add(_client.ViewCharacter(id));
            }
            foreach (var task in detailTasks) {
                details.Add(await task);
            }
            System.Diagnostics.Debug.WriteLine(DateTime.Now - dt);

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
            Submission is CharacterDetail ? $"#weasylcharacter{((CharacterDetail)Submission).charid}"
            : Submission is SubmissionDetail ? $"#weasyl{((SubmissionDetail)Submission).submitid}"
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

		public SubmissionBaseDetail Submission { get; private set; }

        public WeasylSubmissionWrapper(SubmissionBaseDetail submission) {
            Submission = submission;
        }
    }
}
