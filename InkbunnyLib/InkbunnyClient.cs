using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
	public class InkbunnyClient {
		private LoginResponse loginResponse;

		public string Username {
			get {
				return loginResponse.user_id.ToString();
			}
		}

		public InkbunnyClient(string username, string password) {
			HttpWebRequest req = WebRequest.CreateHttp("https://inkbunny.net/api_login.php");
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.UserAgent = "WeasylSync/1.1";
			using (StreamWriter sw = new StreamWriter(req.GetRequestStream())) {
				sw.Write("username=" + WebUtility.UrlEncode(username));
				sw.Write('&');
				sw.Write("password=" + WebUtility.UrlEncode(password));
			}
			using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream())) {
				loginResponse = JsonConvert.DeserializeObject<LoginResponse>(sr.ReadToEnd());
				if (loginResponse.error_code != null) {
					throw new Exception(loginResponse.error_message);
				}
			}
		}

		public async Task<long> Upload(
            //long? submission_id = null,
            //bool? notify = null,
            //long? replace = null,
            //byte[] thumbnail = null,
            IEnumerable<byte[]> files = null
		) {
			string boundary = "----MyAppBoundary" + DateTime.Now.Ticks.ToString("x");

			var request = (HttpWebRequest)WebRequest.Create("https://inkbunny.net/api_upload.php");
			request.ContentType = "multipart/form-data; boundary=" + boundary;
			request.Method = "POST";

            using (var stream = await request.GetRequestStreamAsync()) {
                using (var sw = new StreamWriter(stream)) {
                    //if (submission_id != null) {
                    //    sw.WriteLine("--" + boundary);
                    //    sw.WriteLine("Content-Disposition: form-data; name=\"submission_id\"");
                    //    sw.WriteLine();
                    //    sw.WriteLine(submission_id);
                    //}
                    //if (notify != null) {
                    //    sw.WriteLine("--" + boundary);
                    //    sw.WriteLine("Content-Disposition: form-data; name=\"notify\"");
                    //    sw.WriteLine();
                    //    sw.WriteLine(notify == true ? "true" : "false");
                    //}
                    //if (replace != null) {
                    //    sw.WriteLine("--" + boundary);
                    //    sw.WriteLine("Content-Disposition: form-data; name=\"replace\"");
                    //    sw.WriteLine();
                    //    sw.WriteLine(replace);
                    //}
                    if (files != null) {
                        foreach (byte[] file in files) {
                            sw.WriteLine("--" + boundary);
                            sw.WriteLine("Content-Disposition: form-data; name=\"uploadedfile[]\"; filename=\"a.png\"");
                            sw.WriteLine();
                            sw.Flush();
                            stream.Write(file, 0, file.Length);
                            stream.Flush();
                            sw.WriteLine();
                        }
                    }
                    //if (thumbnail != null) {
                    //    sw.WriteLine("--" + boundary);
                    //    sw.WriteLine("Content-Disposition: form-data; name=\"uploadedthumbnail[]\"; filename=\"\"");
                    //    sw.WriteLine();
                    //    sw.Flush();
                    //    stream.Write(thumbnail, 0, thumbnail.Length);
                    //    stream.Flush();
                    //    sw.WriteLine();
                    //}

                    // The submission only seems to upload properly when I put this last...
                    sw.WriteLine("--" + boundary);
                    sw.WriteLine("Content-Disposition: form-data; name=\"sid\"");
                    sw.WriteLine();
                    sw.WriteLine(loginResponse.sid);
                    sw.WriteLine("--" + boundary + "--");
                    sw.Flush();
                }
            }

            using (var response = await request.GetResponseAsync()) {
				using (var responseStream = response.GetResponseStream()) {
					using (var sr = new StreamReader(responseStream)) {
						UploadResponse r = JsonConvert.DeserializeObject<UploadResponse>(sr.ReadToEnd());
						if (r.error_code != null) {
							throw new Exception(r.error_message);
						}
						return r.submission_id;
					}
				}
			}
		}
	}
}
