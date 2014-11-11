using DontPanic.TumblrSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSync {
	public class PostComponents {
		public BinaryFile ImageData { get; set; }

		public string Header { get; set; }
		public string Body { get; set; }
		public string Footer { get; set; }

		public string Title { get; set; }
		public string URL { get; set; }

		public List<string> Tags { get; private set; }

		public DateTimeOffset? PostDate { get; set; }

		public PostComponents() {
			Tags = new List<string>();
		}

		public void SetTags(params string[] taglists) {
			Tags.Clear();
			foreach (string taglist in taglists) {
				Tags.AddRange(taglist.Replace("#", "").Split(' ').Where(s => s != ""));
			}
		}

		public string CompileHTML() {
			StringBuilder html = new StringBuilder();

			if (Header != null) {
				html.Append(Header);
			}

			if (Body != null) {
				html.Append(Body);
			}

			if (Footer != null) {
				html.Append(Footer);
			}

			html.Replace("{TITLE}", WebUtility.HtmlEncode(Title)).Replace("{URL}", URL);

			return html.ToString();
		}

		public PostData ToPost() {
			PostData post;
			if (ImageData != null) {
				post = PostData.CreatePhoto(new BinaryFile[] { ImageData }, CompileHTML(), URL, Tags);
			} else {
				post = PostData.CreateText(CompileHTML(), null, Tags);
			}
			post.Date = PostDate;
			return post;
		}
	}
}
