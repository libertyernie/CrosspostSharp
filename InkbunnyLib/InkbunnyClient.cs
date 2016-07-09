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
			string boundary = "----WeasylSync" + DateTime.Now.Ticks.ToString("x");

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
                    //    sw.WriteLine(notify == true ? "yes" : "no");
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

        public async Task<EditSubmissionResponse> EditSubmission(
            long submission_id,
            string title = null,
            string desc = null,
            string story = null,
            bool convert_html_entities = false,
            SubmissionType? type = null,
            bool? scraps = null,
            bool? use_twitter = null,
            TwitterImagePref? twitter_image_pref = null,
            bool? isPublic = null,
            bool notifyWatchersWhenPublic = false,
            IEnumerable<string> keywords = null,
            InkbunnyRating tag = null,
            bool guest_block = false,
            bool friends_only = false
        ) {
            string boundary = "----WeasylSync" + DateTime.Now.Ticks.ToString("x");

            var request = (HttpWebRequest)WebRequest.Create("https://inkbunny.net/api_editsubmission.php");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";

            using (var stream = await request.GetRequestStreamAsync()) {
                using (var sw = new StreamWriter(stream)) {
                    sw.WriteLine("--" + boundary);
                    sw.WriteLine("Content-Disposition: form-data; name=\"submission_id\"");
                    sw.WriteLine();
                    sw.WriteLine(submission_id);
                    if (title != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"title\"");
                        sw.WriteLine();
                        sw.WriteLine(title);
                    }
                    if (desc != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"desc\"");
                        sw.WriteLine();
                        sw.WriteLine(desc);
                    }
                    if (story != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"story\"");
                        sw.WriteLine();
                        sw.WriteLine(story);
                    }
                    sw.WriteLine("--" + boundary);
                    sw.WriteLine("Content-Disposition: form-data; name=\"convert_html_entities\"");
                    sw.WriteLine();
                    sw.WriteLine(convert_html_entities ? "yes" : "no");
                    if (type != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"type\"");
                        sw.WriteLine();
                        sw.WriteLine((int)type);
                    }
                    if (scraps != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"scraps\"");
                        sw.WriteLine();
                        sw.WriteLine(scraps == true ? "yes" : "no");
                    }
                    if (use_twitter != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"use_twitter\"");
                        sw.WriteLine();
                        sw.WriteLine(use_twitter == true ? "yes" : "no");
                    }
                    if (twitter_image_pref != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"twitter_image_pref\"");
                        sw.WriteLine();
                        sw.WriteLine((int)twitter_image_pref);
                    }
                    if (isPublic != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"visibility\"");
                        sw.WriteLine();
                        sw.WriteLine(isPublic == false ? "no"
                            : notifyWatchersWhenPublic ? "yes" : "yes_nowatch");
                    }
                    if (keywords != null) {
                        sw.WriteLine("--" + boundary);
                        sw.WriteLine("Content-Disposition: form-data; name=\"keywords\"");
                        sw.WriteLine();
                        sw.WriteLine(string.Join(" ", keywords.Select(s => s.Replace(',', '_').Replace(' ', '_'))));
                    }
                    if (tag != null) {
                        for (int i=2; i<=5; i++) {
                            sw.WriteLine("--" + boundary);
                            sw.WriteLine("Content-Disposition: form-data; name=\"tag[" + i + "]\"");
                            sw.WriteLine();
                            sw.WriteLine(tag[i]);
                        }
                    }
                    sw.WriteLine("--" + boundary);
                    sw.WriteLine("Content-Disposition: form-data; name=\"guest_block\"");
                    sw.WriteLine();
                    sw.WriteLine(guest_block == true ? "yes" : "no");
                    sw.WriteLine("--" + boundary);
                    sw.WriteLine("Content-Disposition: form-data; name=\"friends_only\"");
                    sw.WriteLine();
                    sw.WriteLine(friends_only == true ? "yes" : "no");

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
                        var r = JsonConvert.DeserializeObject<EditSubmissionResponse>(sr.ReadToEnd());
                        if (r.error_code != null) {
                            throw new Exception(r.error_message);
                        }
                        return r;
                    }
                }
            }
        }
	}
}
