using DeviantartApi.Objects.SubObjects.StashDelta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using DeviantartApi.Objects;

namespace ArtSourceWrapper {
    public class StashWrapper : SiteWrapper<StashSubmissionWrapper, uint> {
        public override string SiteName => "Sta.sh";
        public override string WrapperName => "DeviantArt Sta.sh";

        public override int BatchSize { get; set; } = 120;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 120;

        public int TotalItemsIncludingStacks { get; private set; } = 0;

        public static async Task<User> GetUserAsync() {
            var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Result.Error)) {
                throw new DeviantArtException(result.Result.ErrorDescription);
            }
            return result.Result;
        }

        private User _user;

        public override async Task<string> WhoamiAsync() {
            if (_user == null) _user = await GetUserAsync();
            return _user.Username;
        }

        public override async Task<string> GetUserIconAsync(int size) {
            if (_user == null) _user = await GetUserAsync();
            return _user.UserIconUrl.AbsoluteUri;
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
            TotalItemsIncludingStacks += result.Result.Entries.Count;
            var q = result.Result.Entries
                    .Where(e => e.ItemId != 0)
                    .Where(e => e.Metadata.Files != null)
					.OrderByDescending(e => e.Metadata.CreationTime)
                    .Select(e => new StashSubmissionWrapper(e));
            return new InternalFetchResult(
                q,
                (uint)(result.Result.NextOffset ?? 0),
                !result.Result.HasMore);
        }
    }

    public class StashOrderedWrapper : SiteWrapper<StashSubmissionWrapper, int> {
        private StashWrapper _wrapper;

        public override string SiteName => _wrapper.SiteName;
        public override string WrapperName => _wrapper.WrapperName + " (newest first)";

        public override int BatchSize { get; set; } = 0;
        public override int MinBatchSize => 0;
        public override int MaxBatchSize => 0;

        public StashOrderedWrapper() {
            _wrapper = new StashWrapper();
        }

        public override Task<string> WhoamiAsync() {
            return _wrapper.WhoamiAsync();
        }

        public override Task<string> GetUserIconAsync(int size) {
            return _wrapper.GetUserIconAsync(size);
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            for (int i=0; i<10; i++) {
                int read = await _wrapper.FetchAsync();
                if (read == -1) break;
            }
            if (!_wrapper.IsEnded) {
                throw new Exception($"You have {_wrapper.TotalItemsIncludingStacks} items in sta.sh (submissions + stacks), which is too many to show here.");
            }

            AsynchronousCachedEnumerable<StashSubmissionWrapper, uint> w = _wrapper;
            return new InternalFetchResult(w.Cache.OrderByDescending(c => c.Timestamp), 0, true);
        }
    }

    public class StashSubmissionWrapper : ISubmissionWrapper {
        private readonly Entry _entry;

        public StashSubmissionWrapper(Entry entry) {
            _entry = entry;
        }

        public string Title => _entry.Metadata.Title;
        public string HTMLDescription => _entry.Metadata.ArtistComment;
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
        public string ImageURL => _entry.Metadata.Files.OrderByDescending(f => f.Width).Select(f => f.Src).FirstOrDefault();
        public string ThumbnailURL => _entry.Metadata.Thumb?.Src ?? ImageURL;
        public Color? BorderColor => null;
        public bool OwnWork => true;

		//public Task DeleteAsync() {
		//	return new DeviantartApi.Requests.Stash.DeleteRequest(checked((int)_entry.ItemId)).ExecuteAsync();
		//}
	}
}
