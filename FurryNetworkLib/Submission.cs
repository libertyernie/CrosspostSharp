using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryNetworkLib {
	public class Submission {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Published { get; set; }
        public DateTime? Made_public_date { get; set; }
        public object Deleted { get; set; }
        public object Hard_deleted { get; set; } // boolean or int
        public bool Processed { get; set; }
        public bool Community_tags_allowed { get; set; }
        public string Record_type { get; set; }
        public object Ticket_id { get; set; }
        public IEnumerable<object> Collection_ids { get; set; }
        public Character Character { get; set; }
        public IEnumerable<object> Tags { get; set; }
        public bool Promoted { get; set; }
        public int Comments { get; set; }
        public int Favorites { get; set; }
        public int Promotes { get; set; }
        public int Promotes_Week { get; set; }
        public int Promotes_Month { get; set; }
        public int Views { get; set; }
        public bool Favorited { get; set; }
        public object Tag_suggest { get; set; }
        public IEnumerable<string> Promote_array { get; set; }

		public IEnumerable<string> TagStrings {
			get {
				foreach (var tag in Tags) {
					if (tag is string s) {
						yield return s;
					} else if (tag is JObject o) {
						string json = o.ToString();
						yield return JsonConvert.DeserializeAnonymousType(json, new {
							value = ""
						}).value;
					}
				}
			}
		}
    }

    public abstract class FileSubmission : Submission {
        public string Md5 { get; set; }
        public string Url { get; set; }
        public string File_name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string Content_type { get; set; }
        public int Size { get; set; }
        public Images Images { get; set; }
    }

    public class Artwork : FileSubmission { }
    public class Photo : FileSubmission { }
    public class Multimedia : FileSubmission { }

    public abstract class TextSubmission : Submission {
        public string Subtitle { get; set; }
        public string Content { get; set; }
    }

    public class Story : TextSubmission { }
    public class Journal : TextSubmission { }
}
