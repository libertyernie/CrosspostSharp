using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSyncLib {
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
        protected abstract string HTMLDescription { get; }

        public int comments { get; set; }
        public bool favorited { get; set; }
        public int favorites { get; set; }
        public bool friends_only { get; set; }
        public string[] tags { get; set; }
        public int views { get; set; }
        
        public string GetDescription(bool makeLinksAbsolute) {
            if (makeLinksAbsolute) {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(HTMLDescription);

                const string baseUrl = "http://www.weasyl.com";

                // Convert all image URLs to absolute URLs
                foreach (var img in doc.DocumentNode.Descendants("img")) {
                    img.Attributes["src"].Value = new Uri(new Uri(baseUrl), img.Attributes["src"].Value).AbsoluteUri;
                }

                // Convert all relative links to absolute URLS
                foreach (var a in doc.DocumentNode.Descendants("a")) {
                    a.Attributes["href"].Value = new Uri(new Uri(baseUrl), a.Attributes["href"].Value).AbsoluteUri;

                    // For links with a user-icon class, apply styles to the img tag within
                    var classAttribute = a.Attributes["class"];
                    if (classAttribute != null && classAttribute.Value.Contains("user-icon")) {
                        foreach (var img in a.Descendants("img")) {
                            img.Attributes.Add("style", "display: inline-block; height: 50px; vertical-align: middle; width: 50px");
                        }
                    }
                }

                StringWriter writer = new StringWriter();
                doc.Save(writer);
                return writer.ToString();
            } else {
                return HTMLDescription;
            }
        }
    }

    public class SubmissionDetail : SubmissionBaseDetail {
        public int submitid { get; set; }
        public string subtype { get; set; }

        public string description { get; set; }
        public string embedlink { get; set; }
        public string folder_name { get; set; }
        public int? folderid { get; set; }

        protected override string HTMLDescription {
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

        protected override string HTMLDescription {
            get {
                return $"<p> Name: {title} <br> Age: {age} <br> Gender: {gender} <br> Height: {height} <br> Weight: {weight} <br> Species: {species} </p> {content}";
            }
        }
    }
}
