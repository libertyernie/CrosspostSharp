using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WeasylLib {
    public static class Scraper {
		public static async Task<List<int>> GetCharacterIdsAsync(string user) {
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
	}
}
