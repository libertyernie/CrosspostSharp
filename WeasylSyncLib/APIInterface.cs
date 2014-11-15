using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSyncLib
{
    public class WeasylAPI {

		private WebClient client;

		public string APIKey {
			set {
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

		public Gallery UserGallery(string user, DateTime? since = null, int? count = null, int? folderid = null, int? backid = null, int? nextid = null) {
			client.QueryString.Clear();
			if (since != null) client.QueryString.Add("since", since.Value.ToString("u"));
			if (count != null) client.QueryString.Add("count", count.ToString());
			if (folderid != null) client.QueryString.Add("folderid", folderid.ToString());
			if (backid != null) client.QueryString.Add("backid", backid.ToString());
			if (nextid != null) client.QueryString.Add("nextid", nextid.ToString());
			string json = client.DownloadString("https://www.weasyl.com/api/users/" + user + "/gallery");
			return JsonConvert.DeserializeObject<Gallery>(json);
		}

		public SubmissionDetail ViewSubmission(Submission submission) {
			return ViewSubmission(submission.submitid);
		}

		public SubmissionDetail ViewSubmission(int submitid) {
			client.QueryString.Clear();
			string json = client.DownloadString("https://www.weasyl.com/api/submissions/" + submitid + "/view");
			return JsonConvert.DeserializeObject<SubmissionDetail>(json);
		}

		/// <summary>
		/// Gets the username and ID of the currently logged in user, if any.
		/// If there is no current user, this function will return null.
		/// </summary>
		public User Whoami() {
			client.QueryString.Clear();
			try {
				string json = client.DownloadString("https://www.weasyl.com/api/whoami");
				return JsonConvert.DeserializeObject<User>(json);
			} catch (WebException e) {
				if (e.Response is HttpWebResponse && ((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized) {
					return null;
				} else {
					throw e;
				}
			}
		}
    }
}
