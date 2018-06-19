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
        /// The batch size will be this amount if possible, or the greatest possible amount.
        /// Changing the batch size after some elements have been fetched may have unexpected effects in some cases.
        /// </summary>
        int BatchSize { get; set; }

        /// <summary>
        /// The minimum batch size for this wrapper. This property is read-only.
        /// </summary>
        int MinBatchSize { get; }

        /// <summary>
        /// The maximum batch size for this wrapper. This property is read-only.
        /// </summary>
        int MaxBatchSize { get; }
		
        /// <summary>
        /// A display name for this wrapper, e.g. "DeviantArt (Scraps)".
        /// </summary>
        string WrapperName { get; }

		/// <summary>
		/// Whether this wrapper is expected to omit some of the posts from
		/// the source, thus returning less than BatchSize items on a regular
		/// basis (for example, tweets or Tumblr posts with no images, or
		/// Inkbunny posts that are not public.)
		/// </summary>
		bool SubmissionsFiltered { get; }

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
        /// Get the URL to the user icon or avatar of the currently logged in user.
        /// </summary>
        /// <param name="size">The preferred width/height, in pixels</param>
        /// <returns>A URL, or null if no image is available</returns>
        Task<string> GetUserIconAsync(int size);

        /// <summary>
        /// Get another batch of submissions and add them to the cache.
        /// </summary>
        /// <returns>The number of entries added to the cache, or -1 if no items were added because all of the submissions are already downloaded.</returns>
        Task<int> FetchAsync();

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
    public abstract class SiteWrapper<TWrapper, TPosition> : AsynchronousCachedEnumerable<TWrapper, TPosition>, ISiteWrapper where TWrapper : ISubmissionWrapper where TPosition : struct {
        /// <summary>
        /// A display name for this wrapper, e.g. "DeviantArt (Scraps)".
        /// </summary>
        public abstract string WrapperName { get; }

		/// <summary>
		/// Whether this wrapper is expected to omit some of the posts from
		/// the source, thus returning less than BatchSize items on a regular
		/// basis (for example, tweets or Tumblr posts with no images, or
		/// Inkbunny posts that are not public.)
		/// </summary>
		public abstract bool SubmissionsFiltered { get; }

        /// <summary>
        /// Looks up the username of the currently logged in user.
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> WhoamiAsync();

        /// <summary>
        /// Get the URL to the user icon or avatar of the currently logged in user.
        /// </summary>
        /// <param name="size">The preferred width/height, in pixels</param>
        /// <returns>A URL, or null if no image is available</returns>
        public abstract Task<string> GetUserIconAsync(int size);

        /// <summary>
        /// A list of the currently cached submissions. Call FetchAsync to get more.
        /// </summary>
        public new IEnumerable<ISubmissionWrapper> Cache {
            get {
                foreach (var w in base.Cache) yield return w;
            }
        }
    }
    
    public interface ISubmissionWrapper {
        string Title { get; }
        string HTMLDescription { get; }
        bool Mature { get; }
		bool Adult { get; }
		IEnumerable<string> Tags { get; }
        DateTime Timestamp { get; }

        string ViewURL { get; }
        string ImageURL { get; }
        string ThumbnailURL { get; }
        Color? BorderColor { get; }
    }

	public interface IStatusUpdate : SourceWrappers.IStatusUpdate { }

	public interface IDeletable : SourceWrappers.IDeletable { }

    public class MetadataModificationSubmissionWrapper : ISubmissionWrapper {
        private ISubmissionWrapper _base;

        public MetadataModificationSubmissionWrapper(
            ISubmissionWrapper from,
            string title = null,
            string htmlDescription = null,
            bool? mature = null,
            IEnumerable<string> tags = null
        ) {
            _base = from;
            this.Title = title ?? from.Title;
            this.HTMLDescription = htmlDescription ?? from.HTMLDescription;
            this.Mature = mature ?? from.Mature;
            this.Tags = tags?.ToList() ?? from.Tags;
        }

        public string Title { get; set; }
        public string HTMLDescription { get; set; }
        public bool Mature { get; set; }
		public bool Adult { get; set; }
		public IEnumerable<string> Tags { get; set; }
        public DateTime Timestamp => _base.Timestamp;

        public string ViewURL => _base.ViewURL;
        public string ImageURL => _base.ImageURL;
        public string ThumbnailURL => _base.ThumbnailURL;
        public Color? BorderColor => _base.BorderColor;
	}
}
