using FAExportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArtSourceWrapper {
    public class FurAffinityIdWrapper : AsynchronousCachedEnumerable<int, int> {
        private readonly FAUserClient _client;
        private readonly FAFolder _folder;

        public override int BatchSize { get; set; } = 60;
        public override int MinBatchSize => 60;
        public override int MaxBatchSize => 60;

        public string WrapperName => _folder == FAFolder.scraps
            ? "FurAffinity (Scraps)"
            : "FurAffinity (Gallery)";

        public FurAffinityIdWrapper(string a, string b, bool scraps = false) {
            _client = new FAUserClient(a, b);
            _folder = scraps ? FAFolder.scraps : FAFolder.gallery;
        }

        public Task<string> WhoamiAsync() {
            return _client.WhoamiAsync();
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            string username = await WhoamiAsync();

            int pos = startPosition ?? 1;
            var result = await _client.GetSubmissionIdsAsync(username, _folder, pos);
            return new InternalFetchResult(result, pos + 1, isEnded: !result.Any());
        }

        public async Task<FurAffinitySubmissionWrapper> GetSubmissionDetails(int id) {
            return new FurAffinitySubmissionWrapper(id, await _client.GetSubmissionAsync(id));
        }
    }

    public class FurAffinityWrapper : SiteWrapper<FurAffinitySubmissionWrapper, int> {
        private readonly FurAffinityIdWrapper _idWrapper;

        public override int BatchSize { get; set; } = 1;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 1;

        public FurAffinityWrapper(FurAffinityIdWrapper idWrapper) {
            _idWrapper = idWrapper;
        }

        public override string SiteName => "FurAffinity";
        public override string WrapperName => _idWrapper.WrapperName;

        public override Task<string> WhoamiAsync() {
            return _idWrapper.WhoamiAsync();
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            int skip = startPosition ?? 0;
            
            while (_idWrapper.Cache.Count() < skip + 1 && !_idWrapper.IsEnded) {
                await _idWrapper.FetchAsync();
            }

            var task = _idWrapper.Cache
                .Skip(skip)
                .Select(id => _idWrapper.GetSubmissionDetails(id))
                .FirstOrDefault();

            var wrappers = task == null
                ? Enumerable.Empty<FurAffinitySubmissionWrapper>()
                : new[] { await task };

            return new InternalFetchResult(wrappers, skip + 1, _idWrapper.Cache.Count() <= skip + 1);
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
        public Color? BorderColor => Color.Orange; // TODO
        public bool OwnWork => true;
    }
}
