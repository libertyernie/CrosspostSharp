using System;
using System.Collections.Generic;

namespace FAExportLib {
	public class FAUser {
		public string Name;
		public string Profile;
		public string Account_type;
		public string Avatar;
		public string FullName;
		public string ArtistType;
		public string RegisteredSince;
		public DateTimeOffset registeredAt;
		public string CurrentMood;
		public string ArtistProfile;
		public int Pageviews;
		public int Submissions;
		public int CommentsRecieved;
		public int CommentsGiven;
		public int Journals;
		public int Favorites;
		public FAUserPageSubmission FeaturedSubmission;
		public FAUserPageSubmission ProfileId;
		public Dictionary<string, string> ArtistInformation;
		public IEnumerable<FAUserContactInformation> ContactInformation;
		public WatchUserCollection Watchers;
		public WatchUserCollection Watching;

		public class FAUserPageSubmission {
			public string Id;
			public string Title;
			public string Thumbnail;
			public string Link;
		}

		public class FAUserContactInformation {
			public string Title;
			public string Name;
			public string Link;
		}

		public class WatchUserCollection {
			public int Count;
			public IEnumerable<WatchUser> Recent;
		}

		public class WatchUser {
			public string Name;
			public string Link;
		}
	}

}
