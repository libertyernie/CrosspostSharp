using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FAExportLib
{
	public class FAUserClient : FAClient {
		private readonly string _cookieA;
		private readonly string _cookieB;

		/// <summary>
		/// Create a client that connects to FAExport as a certain user.
		/// </summary>
		/// <param name="a">FurAffinity's "a" cookie</param>
		/// <param name="b">FurAffinity's "b" cookie</param>
		public FAUserClient(string a, string b) {
			_cookieA = a ?? throw new ArgumentNullException(nameof(a));
			_cookieB = b ?? throw new ArgumentNullException(nameof(b));
		}

		protected override string GetFACookie() {
			return $"b={_cookieB}; a={_cookieA}";
		}

		/// <summary>
		/// Posts a journal to FurAffinity.
		/// </summary>
		/// <param name="title">The title of the journal</param>
		/// <param name="description">The body of the journal</param>
		/// <returns>The URL of the created journal</returns>
		public async Task<string> PostJournalAsync(string title, string description) {
			try {
				var request = WebRequest.CreateHttp("https://faexport.boothale.net/journal.json");
				request.Method = "POST";
				request.UserAgent = UserAgent;
				request.Headers.Add("FA_COOKIE", GetFACookie());
				using (StreamWriter sw = new StreamWriter(await request.GetRequestStreamAsync())) {
					var body = new {
						title,
						description
					};
					await sw.WriteAsync(JsonConvert.SerializeObject(body));
				}
				using (var response = await request.GetResponseAsync())
				using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
					var json = await sr.ReadToEndAsync();
					var o = JsonConvert.DeserializeAnonymousType(json, new {
						url = ""
					});
					return o.url;
				}
			} catch (WebException ex) {
				throw new FAExportException(ex);
			}
		}

		/// <summary>
		/// Get the logged in username (if any) by scraping it from the FurAffinity homepage.
		/// </summary>
		/// <returns>The username (without the leading tilde), or possibly null if the "a" and "b" cookies are not valid.</returns>
		public async Task<string> WhoamiAsync() {
			var url = "https://www.furaffinity.net";
			var request = WebRequest.CreateHttp(url);
			request.UserAgent = UserAgent;
			if (request.CookieContainer == null)
				request.CookieContainer = new CookieContainer();
			if (_cookieA != null & _cookieB != null) {
				request.CookieContainer.Add(new Uri(url), new Cookie("a", _cookieA));
				request.CookieContainer.Add(new Uri(url), new Cookie("b", _cookieB));
			}
			using (var response = await request.GetResponseAsync())
			using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
				string line;
				while ((line = await sr.ReadLineAsync()) != null) {
					if (line.Contains("my-username")) {
						line = line.Substring(line.IndexOf("~") + 1);
						var endTagInd = line.IndexOf("<");
						if (endTagInd >= 0) {
							line = line.Substring(0, endTagInd);
							return line;
						}
					}
				}
				return null;
			}
		}
	}

}
