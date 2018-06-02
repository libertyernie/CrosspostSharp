using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {

	/// <summary>
	/// A class returned from InternalFetchAsync.
	/// </summary>
	public class InternalFetchResult<TElement, TPosition> {
		/// <summary>
		/// Additional items to add to the cache.
		/// </summary>
		public readonly IEnumerable<TElement> AdditionalItems;
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
		public InternalFetchResult(IEnumerable<TElement> additionalItems, TPosition nextPosition, bool isEnded = false) {
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
			this.AdditionalItems = Enumerable.Empty<TElement>();
			this.NextPosition = nextPosition;
			this.IsEnded = isEnded;
		}
	}

	/// <summary>
	/// An abstract class representing a wrapper for an asynchronous process that returns items in batches of one or more elements.
	/// </summary>
	/// <typeparam name="TElement">The type of object returned from the process</typeparam>
	/// <typeparam name="TPosition">The type of object to use for an internal position counter; must be a value type</typeparam>
	public abstract class AsynchronousCachedEnumerable<TElement, TPosition> where TPosition : struct {
        private List<TElement> _cache;
        private TPosition? _nextPosition;
        private bool _isEnded;

        /// <summary>
        /// Contains the elements currently in the cache. Call FetchAsync to get more.
        /// </summary>
        public IEnumerable<TElement> Cache => _cache;
        /// <summary>
        /// If true, the cache contains all of the elements, and no more will be added by calling FetchAsync again.
        /// </summary>
        public bool IsEnded => _isEnded;

        /// <summary>
        /// The batch size will be this amount if possible, or the greatest possible amount.
        /// Changing the batch size after some elements have been fetched may have unexpected effects in some cases.
        /// </summary>
        public abstract int BatchSize { get; set; }

        /// <summary>
        /// The minimum batch size for this wrapper. This property is read-only.
        /// </summary>
        public abstract int MinBatchSize { get; }

        /// <summary>
        /// The maximum batch size for this wrapper. This property is read-only.
        /// </summary>
        public abstract int MaxBatchSize { get; }

        public AsynchronousCachedEnumerable() {
            _cache = new List<TElement>();
        }

        /// <summary>
        /// This method must be implemented by subclasses to populate the cache.
        /// </summary>
        /// <param name="startPosition">An object denoting the position at which to start (or null if this is the first fetch)</param>
        /// <param name="count">Preferred number of elements to fetch</param>
        /// <returns>An object containing zero or more wrappers to be added, the new position, and whether the end has been reached</returns>
        protected abstract Task<InternalFetchResult<TElement, TPosition>> InternalFetchAsync(TPosition? startPosition, int count);

        /// <summary>
        /// Fetch more elements and add them to the cache.
        /// </summary>
        /// <returns>The number of elements added to the cache, or -1 if no new elements were added because they were all already in the cache.</returns>
        public async Task<int> FetchAsync() {
            if (_isEnded) return -1;

            var list = _cache.ToList();
            var result = await InternalFetchAsync(_nextPosition, Math.Max(MinBatchSize, Math.Min(MaxBatchSize, BatchSize)));

            list.AddRange(result.AdditionalItems);
            _cache = list;
            _nextPosition = result.NextPosition;
            _isEnded = result.IsEnded;

            return result.AdditionalItems.Any() ? result.AdditionalItems.Count()
                : result.IsEnded ? -1
                : 0;
        }

        /// <summary>
        /// Clears the cache and resets the internal position and end markers.
        /// </summary>
        public void Clear() {
            _cache.Clear();
            _nextPosition = null;
            _isEnded = false;
        }
    }
}
