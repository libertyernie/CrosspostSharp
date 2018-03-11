using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharpJournal {
	/// <summary>
	/// An interface representing a client wrapper for a site that journals can be posted to.
	/// Normally a class will extend JournalSource&lt;TWrapper, TPosition> instead of implementing this interface directly.
	/// </summary>
	public interface IJournalSource {
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
		/// A list of the currently cached submissions. Call FetchAsync to get more.
		/// </summary>
		IEnumerable<IJournalWrapper> Cache { get; }

		/// <summary>
		/// Whether the cache contains all of the submissions that are available.
		/// </summary>
		bool IsEnded { get; }
		
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
	/// An interface representing a client wrapper for a site that journals can be posted to.
	/// If you want to declare a variable that any type of wrapper can be assigned to, you may want to use IJournalSiteWrapper.
	/// </summary>
	/// <typeparam name="TWrapper">The type of object to wrap submissions in; must derive from IJournalWrapper</typeparam>
	/// <typeparam name="TPosition">The type of object to use for an internal position counter; must be a value type</typeparam>
	public abstract class JournalSource<TWrapper, TPosition> : AsynchronousCachedEnumerable<TWrapper, TPosition>, IJournalSource where TWrapper : IJournalWrapper where TPosition : struct {
		/// <summary>
		/// A list of the currently cached submissions. Call FetchAsync to get more.
		/// </summary>
		public new IEnumerable<IJournalWrapper> Cache {
			get {
				foreach (var w in base.Cache) yield return w;
			}
		}
	}

	public interface IJournalDestination {
		/// <summary>
		/// Looks up the username of the currently logged in user.
		/// </summary>
		/// <returns></returns>
		Task<string> WhoamiAsync();

		/// <summary>
		/// Posts a journal to the site.
		/// </summary>
		/// <param name="title">The title (optional on some sites)</param>
		/// <param name="html">The journal body (HTML)</param>
		/// <param name="teaser">The teaser (Furry Network only)</param>
		Task PostAsync(string title, string html, string teaser);
	}

	public interface IJournalWrapper {
		string Title { get; }
		string HTMLDescription { get; }
		DateTime Timestamp { get; }
		string ViewURL { get; }
	}
}
