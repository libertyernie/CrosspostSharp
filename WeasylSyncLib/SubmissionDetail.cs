using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSyncLib {
	public class SubmissionDetail : Submission {
		public int comments { get; set; }
		public string description { get; set; }
		public string embedlink { get; set; }
		public bool favorited { get; set; }
		public bool favorites { get; set; }
		public string folder_name { get; set; }
		public int? folderid { get; set; }
		public bool friends_only { get; set; }
		public int views { get; set; }

		public string GetDescription(bool makeLinksAbsolute) {
			if (makeLinksAbsolute) {
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(description);

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
				return description;
			}
		}
	}
}
