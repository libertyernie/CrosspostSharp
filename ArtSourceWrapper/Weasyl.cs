using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeasylSyncLib;

namespace ArtSourceWrapper {
    public class WeasylWrapper : IWrapper {
        private WeasylAPI _client;

        private UpdateGalleryParameters _lastUpdateGalleryParameters;
        private int? _backId, _nextId;

        public string SiteName => "Weasyl";

        public WeasylWrapper(string apiKey) {
            _client = new WeasylAPI { APIKey = apiKey };
        }

        public async Task<string> WhoamiAsync() {
            return (await _client.Whoami())?.login;
        }

        public Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
            _lastUpdateGalleryParameters = p;
            return UpdateGalleryInternalAsync();
        }

        public Task<UpdateGalleryResult> NextPageAsync() {
            if (_lastUpdateGalleryParameters == null) {
                throw new Exception("Cannot call NextPage/PreviousPage before UpdateGalleryAsync.");
            }
            return UpdateGalleryInternalAsync(nextId: _nextId);
        }

        public Task<UpdateGalleryResult> PreviousPageAsync() {
            if (_lastUpdateGalleryParameters == null) {
                throw new Exception("Cannot call NextPage/PreviousPage before UpdateGalleryAsync.");
            }
            return UpdateGalleryInternalAsync(backId: _backId);
        }

        private async Task<UpdateGalleryResult> UpdateGalleryInternalAsync(int? backId = null, int? nextId = null) {
            var p = _lastUpdateGalleryParameters;

            List<Task<SubmissionBaseDetail>> detailTasks = new List<Task<SubmissionBaseDetail>>(4);
            string WeasylUsername = await WhoamiAsync();
            if (WeasylUsername != null) {
                if (p.Weasyl_LoadCharacters) {
                    // Scrape from weasyl website
                    List<int> all_ids = await _client.GetCharacterIds(WeasylUsername);
                    p.Progress?.Report(1 / 5f);
                    IEnumerable<int> ids = all_ids;
                    if (backId != null) {
                        ids = ids.Where(id => id > backId);
                    }
                    if (nextId != null) {
                        ids = ids.Where(id => id < nextId);
                    }
                    ids = ids.Take(p.Count);
                    // Determine backid and nextid
                    _backId = all_ids.Any(x => x < ids.Min())
                        ? ids.Min()
                        : (int?)null;
                    _nextId = all_ids.Any(x => x > ids.Max())
                        ? ids.Max()
                        : (int?)null;
                    foreach (int id in ids) {
                        detailTasks.Add(_client.ViewCharacter(id));
                    }
                } else {
                    var result = await _client.UserGallery(WeasylUsername, backid: backId, nextid: nextId, count: p.Count);
                    p.Progress?.Report(1 / 5f);
                    _backId = result.backid;
                    _nextId = result.nextid;
                    IEnumerable<int> ids = result.submissions.Select(s => s.submitid);
                    foreach (int id in ids) {
                        detailTasks.Add(_client.ViewSubmission(id));
                    }
                }
                int completedCount = 1;
                foreach (Task task in detailTasks) {
                    var _ = task.ContinueWith(t => p.Progress?.Report(++completedCount / 5f));
                }
                var details = new List<SubmissionBaseDetail>(detailTasks.Count);
                foreach (var task in detailTasks) {
                    details.Add(await task);
                }
                details = details.OrderByDescending(d => d.posted_at).ToList();
                return new UpdateGalleryResult {
                    Submissions = details.Select(d => (ISubmissionWrapper)new WeasylSubmissionWrapper(d)).ToList(),
                    HasLess = true,
                    HasMore = true
                };
            } else {
                return new UpdateGalleryResult {
                    Submissions = Enumerable.Empty<ISubmissionWrapper>().ToList(),
                    HasLess = false,
                    HasMore = false,
                };
            }
        }
    }

    public class WeasylSubmissionWrapper : ISubmissionWrapper {
        public string Rating => Submission.rating;
        public bool PotentiallySensitive => Rating != "general";

        public string GeneratedUniqueTag =>
            Submission is CharacterDetail ? $"#weasylcharacter{((CharacterDetail)Submission).charid}"
            : Submission is SubmissionDetail ? $"#weasyl{((SubmissionDetail)Submission).submitid}"
            : null;
        public string HTMLDescription => Submission.GetDescription(true);
        public IEnumerable<string> Tags => Submission.tags;
        public DateTime Timestamp => Submission.posted_at;
        public string Title => Submission.title;
        public string URL => Submission.link;

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

        public SubmissionBaseDetail Submission { get; private set; }

        public WeasylSubmissionWrapper(SubmissionBaseDetail submission) {
            Submission = submission;
        }
    }
}
