using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public class WebRequestFactory {
		public static WebRequest Create(string url) {
			var req = WebRequest.Create(url ?? throw new ArgumentNullException(nameof(url)));
			req.Method = "GET";
			if (req is HttpWebRequest httpreq) {
				if (req.RequestUri.Host.EndsWith(".pximg.net")) {
					httpreq.Referer = "https://app-api.pixiv.net/";
				} else {
					httpreq.UserAgent = "CrosspostSharp/3.6 (https://github.com/libertyernie/CrosspostSharp)";
				}
			}
			return req;
		}
	}
}
