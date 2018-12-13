using System;
using System.Collections.Generic;

namespace FAExportLib {
	public class FAFolderSubmission {
		public int Id;
		public string Title;
		public string Thumbnail;
		public string Link;
	}

	public class FASubmission {
		public string Title;
		public string Description;
		public string Name;
		public string Profile;
		public string Link;
		public string Posted;
		public DateTimeOffset PostedAt;
		public string Download;
		public string Thumbnail;
		public string Category;
		public string Theme;
		public string Species;
		public string Gender;
		public int Favorites;
		public int Comments;
		public int Views;
		public string Resolution;
		public string Rating;
		public IEnumerable<string> Keywords;
	}
}
