using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeasylLib {
	public partial class WeasylClient {
		private CookieContainer cookieContainer = new CookieContainer();

		private HttpWebRequest CreateRequest(string url) {
			HttpWebRequest req = WebRequest.CreateHttp(url);
			if (_apiKey != null) req.Headers["X-Weasyl-API-Key"] = _apiKey;
			req.CookieContainer = cookieContainer;
			req.UserAgent = "WeasylLib.Frontend/0.1 (https://https://github.com/libertyernie/WeasylLib)";
			req.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
			return req;
		}

		public struct Folder {
			public int FolderId;
			public string Name;

			public override string ToString() {
				return Name;
			}
		}

		public async Task<IEnumerable<Folder>> GetFoldersAsync() {
			var list = new List<Folder>();

			HttpWebRequest req = CreateRequest("https://www.weasyl.com/submit/visual");
			using (WebResponse resp = await req.GetResponseAsync())
			using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
				string line;
				while ((line = await sr.ReadLineAsync()) != null) {
					if (line.Contains("<select name=\"folderid\"")) {
						break;
					}
				}

				var regex = new Regex(@"<option value=""(\d+)"">([^<]+)</option>");
				while ((line = await sr.ReadLineAsync()) != null) {
					var match = regex.Match(line);
					if (match.Success && int.TryParse(match.Groups[1].Value, out int id)) {
						list.Add(new Folder {
							FolderId = id,
							Name = match.Groups[2].Value
						});
					}
					if (line.Contains("</select>")) break;
				}
			}

			return list;
		}

		public enum SubmissionType {
			Sketch = 1010,
			Traditional = 1020,
			Digital = 1030,
			Animation = 1040,
			Photography = 1050,
			Design_Interface = 1060,
			Modeling_Sculpture = 1070,
			Crafts_Jewelry = 1075,
			Sewing_Knitting = 1078,
			Desktop_Wallpaper = 1080,
			Other = 1999,
		}

		public enum Rating {
			General = 10,
			Mature = 30,
			Explicit = 40,
		}

		public async Task<Uri> UploadVisualAsync(byte[] data, string title, SubmissionType subtype, int? folderid, Rating rating, string content, IEnumerable<string> tags) {
			string boundary = "--------------------" + Guid.NewGuid();

			HttpWebRequest req = CreateRequest("https://www.weasyl.com/submit/visual");
			req.Method = "POST";
			req.ContentType = $"multipart/form-data; boundary={boundary}";
			using (Stream stream = await req.GetRequestStreamAsync())
			using (StreamWriter sw = new StreamWriter(stream)) {
				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync($"Content-Disposition: form-data; name=\"submitfile\"; filename=\"picture.dat\"");
				sw.WriteLine();
				sw.Flush();
				stream.Write(data, 0, data.Length);
				stream.Flush();
				sw.WriteLine();

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync($"Content-Disposition: form-data; name=\"thumbfile\"; filename=\"\"");
				sw.WriteLine();
				sw.WriteLine();

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync("Content-Disposition: form-data; name=\"title\"");
				sw.WriteLine();
				sw.WriteLine(title);

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync("Content-Disposition: form-data; name=\"subtype\"");
				sw.WriteLine();
				sw.WriteLine((int)subtype);

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync("Content-Disposition: form-data; name=\"folderid\"");
				sw.WriteLine();
				sw.WriteLine(folderid);

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync("Content-Disposition: form-data; name=\"rating\"");
				sw.WriteLine();
				sw.WriteLine((int)rating);

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync("Content-Disposition: form-data; name=\"content\"");
				sw.WriteLine();
				sw.WriteLine(content);

				await sw.WriteLineAsync("--" + boundary);
				await sw.WriteLineAsync("Content-Disposition: form-data; name=\"tags\"");
				sw.WriteLine();
				sw.WriteLine(string.Join(" ", tags.Select(s => s.Replace(' ', '_'))));

				await sw.WriteLineAsync("--" + boundary + "--");
			}
			try {
				using (WebResponse resp = await req.GetResponseAsync()) {
					return resp.ResponseUri;
				}
			} catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == (HttpStatusCode)422) {
				using (var sr = new StreamReader(ex.Response.GetResponseStream())) {
					string html = sr.ReadToEnd();
					var m = Regex.Match(html, "<div id=\"error_content\".*?>\\s+<p>(.*?)</p>", RegexOptions.Singleline);
					if (m.Success) html = m.Groups[1].Value.Trim();
					throw new Exception(html, ex);
				}
			}
		}

		public async Task DeleteSubmissionAsync(int submitid) {
			var user = await WhoamiAsync();

			HttpWebRequest req = CreateRequest("https://www.weasyl.com/remove/submission");
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			using (Stream stream = await req.GetRequestStreamAsync())
			using (StreamWriter sw = new StreamWriter(stream)) {
				await sw.WriteLineAsync($"submitid={submitid}");
			}
			using (var resp = await req.GetResponseAsync()) { }
		}
	}
}
