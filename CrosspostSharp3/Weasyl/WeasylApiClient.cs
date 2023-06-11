using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CrosspostSharp3.Weasyl {
	public partial class WeasylClient {
		private readonly string _apiKey;

		public WeasylClient(string apiKey = null) {
			_apiKey = apiKey;
		}

		public class GalleryRequestOptions {
			public DateTimeOffset? since;
			public int? count;
			public int? folderid;
			public int? backid;
			public int? nextid;
		}

		public async Task<string> GetAvatarUrlAsync(string username) {
			HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/useravatar?username={WebUtility.UrlEncode(username)}");
			if (_apiKey != null) req.Headers["X-Weasyl-API-Key"] = _apiKey;
			using (WebResponse resp = await req.GetResponseAsync())
			using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
				return obj["avatar"];
			}
		}
		
		public async Task<WeasylUser> WhoamiAsync() {
			HttpWebRequest req = WebRequest.CreateHttp("https://www.weasyl.com/api/whoami");
			if (_apiKey != null) req.Headers["X-Weasyl-API-Key"] = _apiKey;
			using (WebResponse resp = await req.GetResponseAsync())
			using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<WeasylUser>(json);
			}
		}
	}
}
