using FAExportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArtSourceWrapper {
	public abstract class FurAffinityIdWrapper : AsynchronousCachedEnumerable<int, int> {
		protected readonly FAUserClient _client;

		public abstract string WrapperName { get; }

		public FurAffinityIdWrapper(string a, string b) {
			_client = new FAUserClient(a, b);
		}

		private string _cachedUserName;

		public async Task<string> WhoamiAsync() {
			if (_cachedUserName == null) {
				_cachedUserName = await _client.WhoamiAsync();
			}
			return _cachedUserName;
		}

		public async Task<string> GetUserIconAync() {
			var user = await _client.GetUserAsync(await WhoamiAsync());
			return user.avatar;
		}

		public async Task<FurAffinitySubmissionWrapper> GetSubmissionDetails(int id) {
			return new FurAffinitySubmissionWrapper(id, await _client.GetSubmissionAsync(id));
		}
	}

	public class FurAffinityUserIdWrapper : FurAffinityIdWrapper {
        private readonly FAFolder _folder;

        public override int BatchSize { get; set; } = 60;
        public override int MinBatchSize => 60;
        public override int MaxBatchSize => 60;

        public override string WrapperName => _folder == FAFolder.scraps
            ? "FurAffinity (Scraps)"
            : "FurAffinity (Gallery)";

        public FurAffinityUserIdWrapper(string a, string b, bool scraps = false) : base(a, b) {
            _folder = scraps ? FAFolder.scraps : FAFolder.gallery;
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            string username = await WhoamiAsync();

            int pos = startPosition ?? 1;
            var result = await _client.GetSubmissionIdsAsync(username, _folder, pos);
            return new InternalFetchResult(result, pos + 1, isEnded: !result.Any());
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

        public override Task<string> GetUserIconAsync(int size) {
            return _idWrapper.GetUserIconAync();
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

            return new InternalFetchResult(wrappers, skip + 1, !_idWrapper.Cache.Skip(skip + 1).Any() && _idWrapper.IsEnded);
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
        public string HTMLDescription {
            get {
                string html = _submission.description;
                int index = html.IndexOf("<br><br>");
                if (index > -1) {
                    html = html.Substring(index + 8).TrimStart();
                }
                return html;
            }
        }
        public bool Mature => string.Equals(_submission.rating, "mature", StringComparison.CurrentCultureIgnoreCase);
		public bool Adult => string.Equals(_submission.rating, "adult", StringComparison.CurrentCultureIgnoreCase);
        public IEnumerable<string> Tags => _submission.keywords;
        public DateTime Timestamp => _submission.posted_at.LocalDateTime;
        public string ViewURL => _submission.link;
        public string ImageURL => _submission.download;
        public string ThumbnailURL => _submission.thumbnail;
        public Color? BorderColor => string.Equals(_submission.rating, "adult", StringComparison.CurrentCultureIgnoreCase) ? Color.FromArgb(0x97, 0x1c, 0x1c)
            : string.Equals(_submission.rating, "mature", StringComparison.CurrentCultureIgnoreCase) ? Color.FromArgb(0x69, 0x7c, 0xc1)
            : (Color?)null;
    }
}
