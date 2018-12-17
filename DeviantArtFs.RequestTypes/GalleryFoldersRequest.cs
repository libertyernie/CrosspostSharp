using System;
using System.Collections.Generic;

namespace DeviantArtFs.RequestTypes {
	public class GalleryFoldersRequest {
		/// <summary>
		/// The user to list folders for, if omitted the authenticated user is used
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// The option to include the content count per each gallery folder 
		/// </summary>
		public bool? CalculateSize { get; set; }
		/// <summary>
		/// The pagination offset
		/// </summary>
		public int? Offset { get; set; }
		/// <summary>
		/// The pagination limit
		/// </summary>
		public int? Limit { get; set; }
	}
}
