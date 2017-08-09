using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public interface IWrapper {
        string SiteName { get; }
        Task<string> WhoamiAsync();
        Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p);
        Task<UpdateGalleryResult> NextPageAsync();
        Task<UpdateGalleryResult> PreviousPageAsync();
    }

    public class UpdateGalleryParameters {
        public int Count;
        public bool Weasyl_LoadCharacters;

        public IProgress<double> Progress;

        public UpdateGalleryParameters() {
            Count = 4;
        }
    }

    public abstract class SiteWrapper<T, Position> : IWrapper where T : ISubmissionWrapper where Position : struct {
        public abstract string SiteName { get; }
        public abstract Task<string> WhoamiAsync();

        private List<T> _cache;
        private Position? _nextPosition;

        public IReadOnlyList<T> Cache => _cache;

        public SiteWrapper() {
            _cache = new List<T>();
        }

        protected class InternalFetchResult {
            public readonly IEnumerable<T> AdditionalItems;
            public readonly Position NextPosition;
            public readonly bool IsEnded;

            public InternalFetchResult(IEnumerable<T> additionalItems, Position nextPosition) {
                this.AdditionalItems = additionalItems;
                this.NextPosition = nextPosition;
                this.IsEnded = false;
            }

            public InternalFetchResult(Position nextPosition, bool isEnded = false) {
                this.AdditionalItems = Enumerable.Empty<T>();
                this.NextPosition = nextPosition;
                this.IsEnded = isEnded;
            }
        }

        protected abstract Task<InternalFetchResult> InternalFetchAsync(Position? startPosition);

        public async Task<int> FetchAsync() {
            var list = _cache.ToList();
            var result = await InternalFetchAsync(_nextPosition);

            list.AddRange(result.AdditionalItems);
            _cache = list;
            _nextPosition = result.NextPosition;

            return result.AdditionalItems.Any() ? result.AdditionalItems.Count()
                : result.IsEnded ? -1
                : 0;
        }

        public async Task<List<T>> FetchSliceAsync(int index, int count, IProgress<double> progress = null) {
            int startCount = Cache.Count;
            while (Cache.Count < index + count) {
                progress?.Report(1.0 * (Cache.Count - startCount + 1) / (index + count - startCount));
                if (await FetchAsync() == -1) {
                    // reached end of list
                    break;
                }
            }
            return Cache.Skip(index).Take(count).ToList();
        }

        public void Clear() {
            _cache.Clear();
            _nextPosition = null;
        }

        private UpdateGalleryParameters _lastUpdateGalleryParameters;
        private int _currentOffset;

        Task<string> IWrapper.WhoamiAsync() => WhoamiAsync();

        private async Task<UpdateGalleryResult> Convert() {
            int index = _currentOffset;
            int count = _lastUpdateGalleryParameters.Count;

            bool ended = false;
            int startCount = Cache.Count;
            while (Cache.Count < index + count) {
                _lastUpdateGalleryParameters.Progress?.Report(1.0 * (Cache.Count - startCount + 1) / (index + count - startCount));
                if (await FetchAsync() == -1) {
                    // reached end of list
                    ended = true;
                    break;
                }
            }

            var list = Cache.Skip(index).Take(count).ToList();

            return new UpdateGalleryResult {
                HasLess = _currentOffset > 0,
                HasMore = !ended,
                Submissions = list.Select(w => {
                    ISubmissionWrapper w2 = w;
                    return w2;
                }).ToList()
            };
        }

        Task<UpdateGalleryResult> IWrapper.UpdateGalleryAsync(UpdateGalleryParameters p) {
            _lastUpdateGalleryParameters = p;
            _currentOffset = 0;
            Clear();
            return Convert();
        }

        Task<UpdateGalleryResult> IWrapper.NextPageAsync() {
            _currentOffset += _lastUpdateGalleryParameters.Count;
            return Convert();
        }

        Task<UpdateGalleryResult> IWrapper.PreviousPageAsync() {
            _currentOffset -= _lastUpdateGalleryParameters.Count;
            if (_currentOffset < 0) _currentOffset = 0;
            return Convert();
        }
    }

    public class UpdateGalleryResult {
        public IList<ISubmissionWrapper> Submissions;
        public bool HasLess;
        public bool HasMore;

        internal UpdateGalleryResult() {
            Submissions = new ISubmissionWrapper[0];
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
