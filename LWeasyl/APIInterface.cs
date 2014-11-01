using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LWeasyl
{
    public static class APIInterface
    {
		private static WebClient client = new WebClient();

		public static Gallery UserGallery(string user, DateTime? since = null, int? count = null, int? folderid = null, int? backid = null, int? nextid = null) {
			client.QueryString.Clear();
			if (since != null) client.QueryString.Add("since", since.Value.ToString("u"));
			if (count != null) client.QueryString.Add("count", count.ToString());
			if (folderid != null) client.QueryString.Add("folderid", folderid.ToString());
			if (backid != null) client.QueryString.Add("backid", backid.ToString());
			if (nextid != null) client.QueryString.Add("nextid", nextid.ToString());
			string json = client.DownloadString("https://www.weasyl.com/api/users/" + user + "/gallery");
			return JsonConvert.DeserializeObject<Gallery>(json);
		}

		public static SubmissionDetail ViewSubmission(Submission submission) {
			return ViewSubmission(submission.submitid);
		}

		public static SubmissionDetail ViewSubmission(int submitid) {
			client.QueryString.Clear();
			string json = client.DownloadString("https://www.weasyl.com/api/submissions/" + submitid + "/view");
			return JsonConvert.DeserializeObject<SubmissionDetail>(json);
		}
    }
}
