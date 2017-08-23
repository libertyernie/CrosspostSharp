using DeviantartApi.Objects.SubObjects.StashDelta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArtSourceWrapper {
    public class StashWrapper : SiteWrapper<StashSubmissionWrapper, uint> {
        public override string SiteName => "sta.sh";
        public override string WrapperName => "sta.sh";

        public override int BatchSize { get; set; } = 120;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 120;

        public override async Task<string> WhoamiAsync() {
            var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Result.Error)) {
                throw new DeviantArtException(result.Result.ErrorDescription);
            }
            return result.Result.Username;
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
            var result = await new DeviantartApi.Requests.Stash.DeltaRequest() {
                Offset = startPosition,
                Limit = (uint)count
            }.ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Result.Error)) {
                throw new DeviantArtException(result.Result.ErrorDescription);
            }
            return new InternalFetchResult(
                result.Result.Entries.Select(e => new StashSubmissionWrapper(e)),
                (uint)(result.Result.NextOffset ?? 0),
                !result.Result.HasMore);
        }
    }

    public class StashSubmissionWrapper : ISubmissionWrapper {
        private readonly Entry _entry;

        public StashSubmissionWrapper(Entry entry) {
            _entry = entry;
        }

        public string Title => _entry.Metadata.Title;
        public string HTMLDescription => _entry.Metadata.Description;
        public bool PotentiallySensitive => false;
        public IEnumerable<string> Tags => _entry.Metadata.Tags;
        public DateTime Timestamp => _entry.Metadata.CreationTime;
        public string ViewURL {
            get {
                StringBuilder url = new StringBuilder();
                long itemId = _entry.ItemId;
                while (itemId > 0) {
                    url.Insert(0, "0123456789abcdefghijklmnopqrstuvwxyz"[(int)(itemId % 36)]);
                    itemId /= 36;
                }
                url.Insert(0, "https://sta.sh/0");
                return url.ToString();
            }
        }
        public string ImageURL => _entry.Metadata.Files.Select(f => f.Src).FirstOrDefault();
        public string ThumbnailURL => _entry.Metadata.Thumb.Src;
        public Color? BorderColor => null;
        public bool OwnWork => true;
    }
}
