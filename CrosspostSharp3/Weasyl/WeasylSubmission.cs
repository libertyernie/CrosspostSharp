using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CrosspostSharp3.Weasyl {
	public abstract class WeasylSubmissionBase {
		public string link;
		public WeasylSubmissionMedia media;
		public string owner;
		public string owner_login;
		public DateTime posted_at;
		public string rating;
		public string title;
		public string type;
	}

	public class WeasylGallerySubmission : WeasylSubmissionBase {
		public int submitid;
		public string subtype;
	}
	
	public abstract class WeasylSubmissionBaseDetail : WeasylSubmissionBase, IRemotePhotoPost {
		public abstract string HTMLDescription { get; }

		public int comments;
		public bool favorited;
		public int favorites;
		public bool friends_only;
		public string[] tags;
		public int views;

		string IRemotePhotoPost.ImageURL => media.submission.Select(x => x.url).First();
		string IRemotePhotoPost.ThumbnailURL => media.thumbnail.Select(x => x.url).First();
		string IPostBase.Title => title;
		bool IPostBase.Mature => rating == "mature";
		bool IPostBase.Adult => rating == "explicit";
		IEnumerable<string> IPostBase.Tags => tags;
		DateTime IPostBase.Timestamp => posted_at;
		string IPostBase.ViewURL => link;

		public override string ToString() {
			return posted_at + " " + title;
		}
	}

	public class WeasylSubmissionDetail : WeasylSubmissionBaseDetail {
		public int submitid;
		public string subtype;

		public string description;
		public string embedlink;
		public string folder_name;
		public int? folderid;

		public override string HTMLDescription => description;
	}

	public class WeasylCharacterDetail : WeasylSubmissionBaseDetail {
		public int charid;

		public string age;
		public string content;
		public string gender;
		public string height;
		public string species;
		public string weight;

		public override string HTMLDescription {
			get {
				Func<string, string> h = WebUtility.HtmlEncode;
				return $"<p> Name: {h(title)} <br> Age: {h(age)} <br> Gender: {h(gender)} <br> Height: {h(height)} <br> Weight: {h(weight)} <br> Species: {h(species)} </p> {content}";
			}
		}
	}
}
