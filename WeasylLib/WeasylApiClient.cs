using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeasylLib {
	public partial class WeasylClient {
		private string _apiKey;

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
		
		public async Task<WeasylGallery> GetUserGalleryAsync(string user, GalleryRequestOptions options = null) {
			if (user == null) throw new ArgumentNullException(nameof(user));

			StringBuilder qs = new StringBuilder();
			if (options is GalleryRequestOptions o) {
				if (o.since is DateTimeOffset dt) {
					qs.Append($"&since={dt.UtcDateTime.ToString("s")}Z");
				}

				if (o.count != null)
					qs.Append($"&count={o.count}");
				if (o.folderid != null)
					qs.Append($"&folderid={o.folderid}");
				if (o.backid != null)
					qs.Append($"&backid={o.backid}");
				if (o.nextid != null)
					qs.Append($"&nextid={o.nextid}");
			}

			HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/users/{user}/gallery?{qs}");
			if (_apiKey != null) req.Headers["X-Weasyl-API-Key"] = _apiKey;
			using (WebResponse resp = await req.GetResponseAsync())
			using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<WeasylGallery>(json);
			}
		}
		
		public async Task<WeasylSubmissionDetail> GetSubmissionAsync(int submitid) {
			HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/submissions/{submitid}/view");
			if (_apiKey != null) req.Headers["X-Weasyl-API-Key"] = _apiKey;
			using (WebResponse resp = await req.GetResponseAsync())
			using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<WeasylSubmissionDetail>(json);
			}
		}

		public async Task<WeasylCharacterDetail> GetCharacterAsync(int charid) {
			HttpWebRequest req = WebRequest.CreateHttp($"https://www.weasyl.com/api/characters/{charid}/view");
			if (_apiKey != null) req.Headers["X-Weasyl-API-Key"] = _apiKey;
			using (WebResponse resp = await req.GetResponseAsync())
			using (StreamReader sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<WeasylCharacterDetail>(json);
			}
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
