using System;
using System.Collections.Generic;

namespace DeviantArtFs.RequestTypes {
	public class StashSubmitRequest {
		public readonly string Filename;
		public readonly string ContentType;
		public readonly byte[] Data;

		public StashSubmitRequest(string filename, string contentType, byte[] data) {
			Filename = filename ?? throw new ArgumentNullException(nameof(filename));
			ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
			Data = data ?? throw new ArgumentNullException(nameof(data));
		}

		/// <summary>
		/// The title of the submission
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Additional information about the submission provided by the author
		/// </summary>
		public string ArtistComments { get; set; }
		/// <summary>
		/// An array of tags describing the submission. Letters, numbers and underscore only.
		/// </summary>
		public IEnumerable<string> Tags { get; set; }
		/// <summary>
		/// A link to the original, in case the artwork has already been posted elsewhere. This field can be restricted with a whitelist by editing your deviantART app.
		/// </summary>
		public string OriginalUrl { get; set; }
		/// <summary>
		/// Is the submission being worked on currently. You can use this flag to warn users that the item is being edited and may change if they reload. Note this does NOT provide any type of locking.
		/// </summary>
		public bool? IsDirty { get; set; }
		/// <summary>
		/// The id of an existing Sta.sh submission. This can be used to overwrite files and /metadata of existing submissions. If you make a new API call containing files, the files that were previously associated with the artwork will be replaced by the new ones.
		/// </summary>
		public long? ItemId { get; set; }
		/// <summary>
		/// The name of the stack to create and place the new submission in. Applies to new submissions only. (Ignored if stackid is set)
		/// </summary>
		public string Stack { get; set; }
		/// <summary>
		/// The id of the stack to create and place the new submission in. Applies to new submissions only.
		/// </summary>
		public long? StackId { get; set; }
	}
}
