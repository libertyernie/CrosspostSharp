using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public static class HtmlConversion {
		public static string ConvertHtmlToText(string html) {
			using (var w = new System.Windows.Forms.WebBrowser()) {
				w.Navigate("about:blank");
				w.Document.Write(html);
				return w.Document.Body.InnerText;
			}
		}
	}
}
