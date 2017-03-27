using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSyncLib
{
    public class WeasylAPI {

		private WebClient client;

        private string _apiKey;
		public string APIKey {
			set {
                _apiKey = value;
				if (value == null) {
					client.Headers.Remove("X-Weasyl-API-Key");
				} else {
					client.Headers.Add("X-Weasyl-API-Key", value);
				}
			}
		}

		public WeasylAPI() {
			client = new WebClient();
		}

		public async Task<Gallery> UserGallery(string user, DateTime? since = null, int? count = null, int? folderid = null, int? backid = null, int? nextid = null) {
            client.QueryString.Clear();
			if (since != null) client.QueryString.Add("since", since.Value.ToString("u"));
			if (count != null) client.QueryString.Add("count", count.ToString());
			if (folderid != null) client.QueryString.Add("folderid", folderid.ToString());
			if (backid != null) client.QueryString.Add("backid", backid.ToString());
			if (nextid != null) client.QueryString.Add("nextid", nextid.ToString());
			string json = await client.DownloadStringTaskAsync($"https://www.weasyl.com/api/users/{user}/gallery");
			return JsonConvert.DeserializeObject<Gallery>(json);
        }

        public async Task<List<int>> GetCharacterIds(string user) {
            client.QueryString.Clear();
            string html = await client.DownloadStringTaskAsync($"https://www.weasyl.com/characters/{user}");
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
        
		public async Task<SubmissionBaseDetail> ViewSubmission(int submitid) {
            HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/submissions/{submitid}/view");
            req.Headers["X-Weasyl-API-Key"] = _apiKey;
            WebResponse resp = await req.GetResponseAsync();
            using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<SubmissionDetail>(json);
            }
        }

        public async Task<SubmissionBaseDetail> ViewCharacter(int charid) {
            HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/characters/{charid}/view");
            req.Headers["X-Weasyl-API-Key"] = _apiKey;
            WebResponse resp = await req.GetResponseAsync();
            using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<CharacterDetail>(json);
            }
        }

        /// <summary>
        /// Gets the username and ID of the currently logged in user, if any.
        /// If there is no current user, this function will return null.
        /// </summary>
        public async Task<User> Whoami() {
			client.QueryString.Clear();
			try {
				string json = await client.DownloadStringTaskAsync("https://www.weasyl.com/api/whoami");
				return JsonConvert.DeserializeObject<User>(json);
			} catch (WebException e) {
				if ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized) {
					return null;
				} else {
					throw e;
				}
			}
		}
    }
}
