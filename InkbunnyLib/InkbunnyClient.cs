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
		public string Sid { get; private set; }
		public long UserId { get; private set; }

		public InkbunnyClient(string sid, long userId) {
            Sid = sid;
            UserId = userId;
        }

        public static async Task<InkbunnyClient> Create(string username, string password) {
            HttpWebRequest req = WebRequest.CreateHttp("https://inkbunny.net/api_login.php");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "WeasylSync/1.2";
            using (Stream stream = await req.GetRequestStreamAsync()) {
                using (StreamWriter sw = new StreamWriter(stream)) {
                    await sw.WriteAsync($"username={WebUtility.UrlEncode(username)}&password={WebUtility.UrlEncode(password)}");
                }
            }
            WebResponse response = await req.GetResponseAsync();
            using (Stream stream = response.GetResponseStream()) {
                using (StreamReader sr = new StreamReader(stream)) {
                    string json = await sr.ReadToEndAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(json);
                    if (loginResponse.error_code != null) {
                        throw new Exception(loginResponse.error_message);
                    }
                    return new InkbunnyClient(loginResponse.sid, loginResponse.user_id);
                }
            }
        }

        public async Task<long> Upload(IEnumerable<byte[]> files = null) {
            string boundary = "----WeasylSync" + DateTime.Now.Ticks.ToString("x");

            var request = (HttpWebRequest)WebRequest.Create("https://inkbunny.net/api_upload.php");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";

            using (var stream = await request.GetRequestStreamAsync()) {
                using (var sw = new StreamWriter(stream)) {
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

                    // The submission only seems to upload properly when I put this last...
                    sw.WriteLine("--" + boundary);
                    sw.WriteLine("Content-Disposition: form-data; name=\"sid\"");
                    sw.WriteLine();
                    sw.WriteLine(Sid);
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
            var dict = new Dictionary<string, object> {
                ["submission_id"] = submission_id,
                ["title"] = title,
                ["desc"] = desc,
                ["story"] = story,
                ["convert_html_entities"] = convert_html_entities.ToYesNo(),
                ["type"] = type?.ToString("d"),
                ["scraps"] = scraps?.ToYesNo(),
                ["use_twitter"] = use_twitter?.ToYesNo(),
                ["twitter_image_pref"] = twitter_image_pref?.ToString("d"),
                ["visibility"] = isPublic == false ? "no"
                            : notifyWatchersWhenPublic ? "yes"
                            : "yes_nowatch",
                ["keywords"] = keywords == null
                            ? null
                            : string.Join(" ", keywords.Select(s => s.Replace(',', '_').Replace(' ', '_'))),
                ["friends_only"] = guest_block.ToYesNo(),
                ["friends_only"] = friends_only.ToYesNo(),
            };
            if (tag != null) {
                for (int i = 2; i <= 5; i++) {
                    dict.Add($"tag[{i}]", tag[i]);
                }
            }

            string json = await PostMultipart("https://inkbunny.net/api_editsubmission.php", dict);
            return JsonConvert.DeserializeObject<EditSubmissionResponse>(json);
        }

        public async Task DeleteSubmission(long submission_id) {
            var dict = new Dictionary<string, object> {
                ["submission_id"] = submission_id
            };

            await PostMultipart("https://inkbunny.net/api_delsubmission.php", dict);
        }

        private async Task<string> PostMultipart(string url, Dictionary<string, object> parameters) {
            string boundary = "----WeasylSync" + DateTime.Now.Ticks.ToString("x");

            var request = WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";

            if (!parameters.ContainsKey("sid")) {
                parameters.Add("sid", Sid);
            }

            using (var sw = new StreamWriter(await request.GetRequestStreamAsync())) {
                foreach (var pair in parameters) {
                    if (pair.Key.Contains("\"")) throw new Exception("Cannot include quotation marks in key name");
                    if (pair.Value == null) continue;
                    await sw.WriteLineAsync("--" + boundary);
                    await sw.WriteLineAsync($"Content-Disposition: form-data; name=\"{pair.Key}\"");
                    await sw.WriteLineAsync();
                    await sw.WriteLineAsync(pair.Value.ToString());
                }
                await sw.WriteLineAsync("--" + boundary + "--");
                await sw.FlushAsync();
            }

            using (var response = await request.GetResponseAsync()) {
                using (var responseStream = response.GetResponseStream()) {
                    using (var sr = new StreamReader(responseStream)) {
                        string json = await sr.ReadToEndAsync();
                        var r = JsonConvert.DeserializeObject<InkbunnyResponse>(json);
                        if (r.error_code != null) {
                            throw new Exception(r.error_message);
                        }
                        return json;
                    }
                }
            }
        }

        private async Task<SearchResponse> Search(Dictionary<string, object> parameters) {
            string json = await PostMultipart("https://inkbunny.net/api_search.php", parameters);
            return JsonConvert.DeserializeObject<SearchResponse>(json);
        }

        public class CannotDetermineUsernameException : Exception {
            public CannotDetermineUsernameException() : base("Cannot determine your Inkbunny username. Try uploading a submission to Inkbunny first.") { }
        }

        public async Task<string> GetUsername() {
            var response = await Search(UserId, 1);
            if (!response.submissions.Any()) throw new CannotDetermineUsernameException();
            return response.submissions.First().username;
        }

        public Task<IEnumerable<InkbunnySubmission>> SearchByMD5(byte[] md5Hash) {
            return SearchByMD5(string.Join("", md5Hash.Select(b => ((int)b).ToString("x2"))));
        }

        public async Task<IEnumerable<InkbunnySubmission>> SearchByMD5(string md5Hash) {
            var resp = await Search(new Dictionary<string, object> {
                ["text"] = md5Hash,
                ["keywords"] = "no",
                ["md5"] = "yes"
            });
            return resp.submissions;
        }

        public async Task<IEnumerable<InkbunnySubmission>> SearchByKeyword(string keyword) {
            var resp = await Search(new Dictionary<string, object> {
                ["text"] = keyword
            });
            return resp.submissions;
        }

        public Task<SearchResponse> Search(long user_id, int? count = null) {
            return Search(new Dictionary<string, object> {
                ["user_id"] = user_id,
                ["submissions_per_page"] = count,
                ["get_rid"] = "yes"
            });
        }

        public Task<SearchResponse> Search(SearchResponse resp, int page, int? count = null) {
            return Search(new Dictionary<string, object> {
                ["rid"] = resp.rid,
                ["submissions_per_page"] = count,
                ["page"] = page
            });
        }
    }
}
