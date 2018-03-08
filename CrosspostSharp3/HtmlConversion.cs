using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public static class HtmlConversion {
		public static async Task<string> ConvertHtmlToMarkdown(string html) {
			try {
				var req = WebRequestFactory.Create("https://fuckyeahmarkdown.com/go/");
				if (req is HttpWebRequest hreq) {
					hreq.ServerCertificateValidationCallback += (a, b, c, d) => true;
				}
				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";
				using (var sw = new StreamWriter(await req.GetRequestStreamAsync())) {
					sw.Write($"html={WebUtility.UrlEncode(html)}");
				}
				using (var resp = await req.GetResponseAsync())
				using (var sr = new StreamReader(resp.GetResponseStream())) {
					return await sr.ReadToEndAsync();
				}
			} catch (WebException ex) {
				Console.Error.WriteLine("Could not convert HTML to Markdown: " + ex.Message);
				return ConvertHtmlToText(html);
			}
		}

		public static string ConvertHtmlToText(string html) {
			using (var w = new System.Windows.Forms.WebBrowser()) {
				w.Navigate("about:blank");
				w.Document.Write(html);
				return w.Document.Body.InnerText;
			}
		}
	}
}
