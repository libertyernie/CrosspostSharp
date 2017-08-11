using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    /// <summary>
    /// An interface representing a client wrapper for an art site.
    /// Normally a class will extend SiteWrapper&lt;TWrapper, TPosition> instead of implementing this interface directly.
    /// </summary>
    public interface ISiteWrapper {
        /// <summary>
        /// The name of the site this wrapper is for (to be shown to the user.)
        /// </summary>
        string SiteName { get; }

        /// <summary>
        /// A list of the currently cached submissions. Call FetchAsync to get more.
        /// </summary>
        IEnumerable<ISubmissionWrapper> Cache { get; }

        /// <summary>
        /// Whether the cache contains all of the submissions that are available.
        /// </summary>
        bool IsEnded { get; }

        /// <summary>
        /// Find the username of the currently logged in user.
        /// </summary>
        /// <returns>The username of the currently logged in user</returns>
        Task<string> WhoamiAsync();

        /// <summary>
        /// Get another batch of submissions and add them to the cache.
        /// </summary>
        /// <param name="maxCount">If specified, the batch size will this amount if possible, or the greatest possible amount. If null, the batch size depends on the wrapper and the source site.</param>
        /// <returns>The number of entries added to the cache, or -1 if no items were added because all of the submissions are already downloaded.</returns>
        Task<int> FetchAsync(ushort? maxCount = null);

        /// <summary>
        /// Clears the cache and resets the internal position.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// An interface representing a client wrapper for an art site.
    /// If you want to declare a variable that any type of wrapper can be assigned to, you may want to use ISiteWrapper.
    /// </summary>
    /// <typeparam name="TWrapper">The type of object to wrap submissions in; must derive from TWrapper</typeparam>
    /// <typeparam name="TPosition">The type of object to use for an internal position counter; must be a value type</typeparam>
    public abstract class SiteWrapper<TWrapper, TPosition> : ISiteWrapper where TWrapper : ISubmissionWrapper where TPosition : struct {
        public abstract string SiteName { get; }
        public abstract Task<string> WhoamiAsync();

        private List<TWrapper> _cache;
        private TPosition? _nextPosition;
        private bool _isEnded;

        public IEnumerable<ISubmissionWrapper> Cache {
            get {
                foreach (var w in _cache) yield return w;
            }
        }
        public bool IsEnded => _isEnded;

        public SiteWrapper() {
            _cache = new List<TWrapper>();
        }

        /// <summary>
        /// A class returned from InternalFetchAsync.
        /// </summary>
        protected class InternalFetchResult {
            /// <summary>
            /// Additional items to add to the cache.
            /// </summary>
            public readonly IEnumerable<TWrapper> AdditionalItems;
            /// <summary>
            /// The new position.
            /// </summary>
            public readonly TPosition NextPosition;
            /// <summary>
            /// Whether the end of the list of submissions has been reached. If true, the position should no longer matter.
            /// </summary>
            public readonly bool IsEnded;

            /// <summary>
            /// Create a new InternalFetchResult with a list of items.
            /// </summary>
            /// <param name="additionalItems">Additional items to add to the cache.</param>
            /// <param name="nextPosition">The new position.</param>
            /// <param name="isEnded">Whether the end of the list of submissions has been reached. If true, the position should no longer matter.</param>
            public InternalFetchResult(IEnumerable<TWrapper> additionalItems, TPosition nextPosition, bool isEnded = false) {
                this.AdditionalItems = additionalItems;
                this.NextPosition = nextPosition;
                this.IsEnded = isEnded;
            }

            /// <summary>
            /// Create a new InternalFetchResult that does not contain any items.
            /// </summary>
            /// <param name="nextPosition">The new position.</param>
            /// <param name="isEnded">Whether the end of the list of submissions has been reached. If true, the position should no longer matter.</param>
            public InternalFetchResult(TPosition nextPosition, bool isEnded = false) {
                this.AdditionalItems = Enumerable.Empty<TWrapper>();
                this.NextPosition = nextPosition;
                this.IsEnded = isEnded;
            }
        }

        /// <summary>
        /// This method must be implemented by subclasses to populate the cache.
        /// </summary>
        /// <param name="startPosition">An object denoting the position at which to start (or null if this is the first fetch)</param>
        /// <param name="maxCount">The maxmimum batch size (if null, the subclass should choose a default)</param>
        /// <returns>An object containing zero or more wrappers to be added, the new position, and whether the end has been reached</returns>
        protected abstract Task<InternalFetchResult> InternalFetchAsync(TPosition? startPosition, ushort? maxCount);

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
