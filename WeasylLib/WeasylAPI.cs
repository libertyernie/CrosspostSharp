using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeasylLib
{
    public class WeasylAPI {
		public string APIKey { private get; set; }

		public async Task<Gallery> GetUserGalleryAsync(string user, DateTime? since = null, int? count = null, int? folderid = null, int? backid = null, int? nextid = null) {
            if (user == null) throw new ArgumentNullException(nameof(user));

            StringBuilder qs = new StringBuilder();
            if (since != null) qs.Append($"&since={since.Value.ToString("u")}");
            if (count != null) qs.Append($"&count={count}");
            if (folderid != null) qs.Append($"&folderid={folderid}");
            if (backid != null) qs.Append($"&backid={backid}");
            if (nextid != null) qs.Append($"&nextid={nextid}");

            HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/users/{user}/gallery?{qs}");
            req.Headers["X-Weasyl-API-Key"] = APIKey;
            using (WebResponse resp = await req.GetResponseAsync())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Gallery>(json);
            }
        }

        public async Task<List<int>> ScrapeCharacterIdsAsync(string user) {
            if (user == null) throw new ArgumentNullException(nameof(user));

            HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/characters/{user}");
            using (WebResponse resp = await req.GetResponseAsync())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                string html = await sr.ReadToEndAsync();
                int lastIndex = 0;
                List<int> ids = new List<int>();
                while ((lastIndex = html.IndexOf("\"/character/", lastIndex)) != -1) {
                    lastIndex += "\"/character/".Length;
                    int id = 0;
                    while (true) {
                        char c = html[lastIndex];
                        if (c < '0' || c > '9') break;
                        id = (10 * id) + (c - '0');
                        lastIndex++;
                    }
                    if (id != 0 && !ids.Contains(id)) ids.Add(id);
                }
                return ids;
            }
        }
        
		public async Task<SubmissionBaseDetail> GetSubmissionAsync(int submitid) {
            HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/submissions/{submitid}/view");
            req.Headers["X-Weasyl-API-Key"] = APIKey;
            using (WebResponse resp = await req.GetResponseAsync())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<SubmissionDetail>(json);
            }
        }

        public async Task<SubmissionBaseDetail> GetCharacterAsync(int charid) {
            HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/characters/{charid}/view");
            req.Headers["X-Weasyl-API-Key"] = APIKey;
            using (WebResponse resp = await req.GetResponseAsync())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<CharacterDetail>(json);
            }
        }

        /// <summary>
        /// Gets the username and ID of the currently logged in user, if any.
        /// If there is no current user, this function will return null.
        /// </summary>
        public async Task<User> WhoamiAsync() {
			try {
                HttpWebRequest req = WebRequest.CreateHttp("https://www.weasyl.com/api/whoami");
                req.Headers["X-Weasyl-API-Key"] = APIKey;
                using (WebResponse resp = await req.GetResponseAsync())
                using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                    string json = await sr.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<User>(json);
                }
			} catch (WebException e) when ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized) {
				return null;
			}
		}
    }
}
