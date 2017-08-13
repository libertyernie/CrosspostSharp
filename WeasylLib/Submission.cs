using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylLib {
	public abstract class SubmissionBase {
		public string link { get; set; }
		public SubmissionMedia media { get; set; }
		public string owner { get; set; }
		public string owner_login { get; set; }
		public DateTime posted_at { get; set; }
		public string rating { get; set; }
		public string title { get; set; }
		public string type { get; set; }

		public override string ToString() {
			return posted_at + " " + title;
        }
    }

    public class GallerySubmission : SubmissionBase {
        public int submitid { get; set; }
        public string subtype { get; set; }
    }
    
    public abstract class SubmissionBaseDetail : SubmissionBase {
        public abstract string HTMLDescription { get; }

        public int comments { get; set; }
        public bool favorited { get; set; }
        public int favorites { get; set; }
        public bool friends_only { get; set; }
        public string[] tags { get; set; }
        public int views { get; set; }
    }

    public class SubmissionDetail : SubmissionBaseDetail {
        public int submitid { get; set; }
        public string subtype { get; set; }

        public string description { get; set; }
        public string embedlink { get; set; }
        public string folder_name { get; set; }
        public int? folderid { get; set; }

        public override string HTMLDescription {
            get {
                return description;
            }
        }
    }

    public class CharacterDetail : SubmissionBaseDetail {
        public int charid { get; set; }

        public string age { get; set; }
        public string content { get; set; }
        public string gender { get; set; }
        public string height { get; set; }
        public string species { get; set; }
        public string weight { get; set; }

        public override string HTMLDescription {
            get {
                return $"<p> Name: {title} <br> Age: {age} <br> Gender: {gender} <br> Height: {height} <br> Weight: {weight} <br> Species: {species} </p> {content}";
            }
        }
    }
}
