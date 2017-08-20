using FAExportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArtSourceWrapper {
    public class FurAffinityGalleryIdWrapper : BaseWrapper<int, int> {
        private readonly FAUserClient _client;
        private readonly FAFolder _folder;

        public FurAffinityGalleryIdWrapper(string a, string b, bool scraps = false) {
            _client = new FAUserClient(a, b);
            _folder = scraps ? FAFolder.scraps : FAFolder.gallery;
        }

        public override Task<string> WhoamiAsync() {
            return _client.WhoamiAsync();
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, ushort? maxCount) {
            string username = await WhoamiAsync();

            int pos = startPosition ?? 1;
            var result = await _client.GetSubmissionIdsAsync(username, _folder, pos);
            return new InternalFetchResult(result, pos + 1, isEnded: !result.Any());
        }

        public async Task<FurAffinitySubmissionWrapper> GetSubmissionDetails(int id) {
            return new FurAffinitySubmissionWrapper(id, await _client.GetSubmissionAsync(id));
        }
    }

    public class FurAffinityGalleryWrapper : SiteWrapper<FurAffinitySubmissionWrapper, int> {
        private readonly FurAffinityGalleryIdWrapper _idWrapper;
        private bool _scraps;

        public FurAffinityGalleryWrapper(string a, string b, bool scraps = false) {
            _idWrapper = new FurAffinityGalleryIdWrapper(a, b, scraps);
            _scraps = scraps;
        }

        public override string SiteName => _scraps ? "FurAffinity (Scraps)" : "FurAffinity";

        public override Task<string> WhoamiAsync() {
            return _idWrapper.WhoamiAsync();
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, ushort? maxCount) {
            int skip = startPosition ?? 0;
            int take = maxCount ?? 3;
            
            while (_idWrapper.Cache.Count() < skip + take && !_idWrapper.IsEnded) {
                await _idWrapper.FetchAsync();
            }

            var tasks = _idWrapper.Cache
                .Skip(skip)
                .Take(take)
                .Select(id => _idWrapper.GetSubmissionDetails(id))
                .ToArray();
            var wrappers = await Task.WhenAll(tasks);

            return new InternalFetchResult(wrappers, skip + take, _idWrapper.Cache.Count() <= skip + take);
        }
    }

    public class FurAffinitySubmissionWrapper : ISubmissionWrapper {
        private int _id;
        private FASubmission _submission;

        public FurAffinitySubmissionWrapper(int id, FASubmission submission) {
            _id = id;
            _submission = submission;
        }

        public string Title => _submission.title;
        public string HTMLDescription => _submission.description;
        public bool PotentiallySensitive => !string.Equals(_submission.rating, "general", StringComparison.CurrentCultureIgnoreCase);
        public IEnumerable<string> Tags => _submission.keywords;
        public string GeneratedUniqueTag => $"#furaffinity{_id}";
        public DateTime Timestamp => _submission.posted_at.LocalDateTime;
        public string ViewURL => _submission.link;
        public string ImageURL => _submission.download;
        public string ThumbnailURL => _submission.thumbnail;
        public Color? BorderColor => null; // TODO
        public bool OwnWork => true;
    }
}
