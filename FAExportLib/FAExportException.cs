using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace FAExportLib {
	public class FAExportException : Exception {
		public readonly HttpStatusCode? StatusCode;
		public readonly string FAError;
		public readonly string FAUrl;

		public FAExportException(WebException ex) : base(ex.Message, ex) {
			StatusCode = (ex.Response as HttpWebResponse)?.StatusCode;
			try {
				using (var sr = new StreamReader(ex.Response.GetResponseStream())) {
					string json = sr.ReadToEnd();
					var o = JsonConvert.DeserializeAnonymousType(json, new {
						error = "",
						url = ""
					});
					FAError = o.error;
					FAUrl = o.url;
				}
			} catch (Exception) { }
		}
	}
}
