using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public interface ISiteWrapper {
        string SiteName { get; }
        IEnumerable<ISubmissionWrapper> Cache { get; }
        bool IsEnded { get; }
        Task<string> WhoamiAsync();
        Task<int> FetchAsync(ushort? maxCount = null);
        void Clear();
    }

    public abstract class SiteWrapper<T, Position> : ISiteWrapper where T : ISubmissionWrapper where Position : struct {
        public abstract string SiteName { get; }
        public abstract Task<string> WhoamiAsync();

        private List<T> _cache;
        private Position? _nextPosition;
        private bool _isEnded;

        public IEnumerable<ISubmissionWrapper> Cache {
            get {
                foreach (var w in _cache) yield return w;
            }
        }
        public bool IsEnded => _isEnded;

        public SiteWrapper() {
            _cache = new List<T>();
        }

        protected class InternalFetchResult {
            public readonly IEnumerable<T> AdditionalItems;
            public readonly Position NextPosition;
            public readonly bool IsEnded;

            public InternalFetchResult(IEnumerable<T> additionalItems, Position nextPosition, bool isEnded = false) {
                this.AdditionalItems = additionalItems;
                this.NextPosition = nextPosition;
                this.IsEnded = isEnded;
            }

            public InternalFetchResult(Position nextPosition, bool isEnded = false) {
                this.AdditionalItems = Enumerable.Empty<T>();
                this.NextPosition = nextPosition;
                this.IsEnded = isEnded;
            }
        }

        protected abstract Task<InternalFetchResult> InternalFetchAsync(Position? startPosition, ushort? maxCount);

        public async Task<int> FetchAsync(ushort? maxCount = null) {
            if (_isEnded) return -1;

            var list = _cache.ToList();
            var result = await InternalFetchAsync(_nextPosition, maxCount);

            list.AddRange(result.AdditionalItems);
            _cache = list;
            _nextPosition = result.NextPosition;
            _isEnded = result.IsEnded;

            return result.AdditionalItems.Any() ? result.AdditionalItems.Count()
                : result.IsEnded ? -1
                : 0;
        }

        public void Clear() {
            _cache.Clear();
            _nextPosition = null;
            _isEnded = false;
        }
    }
    
    public interface ISubmissionWrapper {
        string Title { get; }
        string HTMLDescription { get; }
        bool PotentiallySensitive { get; }
        IEnumerable<string> Tags { get; }
        string GeneratedUniqueTag { get; }
        DateTime Timestamp { get; }

        string ViewURL { get; }
        string ImageURL { get; }
        string ThumbnailURL { get; }
        Color? BorderColor { get; }

		bool OwnWork { get; }
    }
}
