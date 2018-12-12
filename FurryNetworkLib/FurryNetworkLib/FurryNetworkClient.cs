using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FurryNetworkLib {
	public class FurryNetworkClient {
        private string AccessToken { get; set; }
        public string RefreshToken { get; private set; }

        public FurryNetworkClient(string refreshToken = null) {
            RefreshToken = refreshToken;
        }

        private async Task<HttpWebRequest> CreateRequest(string method, string urlPath, object jsonBody = null) {
            var req = WebRequest.CreateHttp("https://beta.furrynetwork.com/api/" + urlPath);
            req.Method = method;
            req.UserAgent = "FurryNetworkLib/0.2 (https://www.github.com/libertyernie/FurryNetworkLib)";
            if (AccessToken != null) {
                req.Headers["Authorization"] = $"Bearer {AccessToken}";
            }
            if (jsonBody != null) {
                req.ContentType = "application/json";
                using (var sw = new StreamWriter(await req.GetRequestStreamAsync())) {
					string json = JsonConvert.SerializeObject(jsonBody, new JsonSerializerSettings {
						ContractResolver = new CamelCasePropertyNamesContractResolver()
					});
					await sw.WriteAsync(json);
                }
            }
            return req;
        }

        private async Task<WebResponse> ExecuteRequest(string method, string urlPath, object jsonBody = null) {
            if (AccessToken == null && RefreshToken != null) {
                await GetNewAccessToken();
            }

            try {
                var req = await CreateRequest(method, urlPath, jsonBody);
                return await req.GetResponseAsync();
            } catch (WebException ex) when (RefreshToken != null && (ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized) {
                await GetNewAccessToken();

                var req = await CreateRequest(method, urlPath, jsonBody);
                return await req.GetResponseAsync();
            }
        }

        /// <summary>
        /// Create a FurryNetworkClient object with a username and password.
        /// </summary>
        /// <param name="username">Your FurryNetwork username</param>
        /// <param name="password">Your FurryNetwork password</param>
        public static async Task<FurryNetworkClient> LoginAsync(string username, string password) {
            var req = WebRequest.CreateHttp("https://beta.furrynetwork.com/api/oauth/token");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "application/json";
            req.UserAgent = "FurryNetworkLib/0.1 (https://www.github.com/libertyernie/FurryNetworkLib)";
            using (var sw = new StreamWriter(await req.GetRequestStreamAsync())) {
                await sw.WriteAsync($"username={WebUtility.UrlEncode(username)}&");
                await sw.WriteAsync($"password={WebUtility.UrlEncode(password)}&");
                await sw.WriteAsync($"grant_type=password&client_id=123&client_secret=");
                await sw.FlushAsync();
            }
            using (var resp = await req.GetResponseAsync())
            using (var sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                var obj = JsonConvert.DeserializeAnonymousType(json, new {
                    access_token = "",
                    expires_in = 0,
                    token_type = "",
                    refresh_token = "",
                    user_id = 0
                });
                if (obj.token_type != "bearer") {
                    throw new Exception("Token returned was not a bearer token");
                }
                return new FurryNetworkClient {
                    AccessToken = obj.access_token,
                    RefreshToken = obj.refresh_token
                };
            }
        }

		public class TokenException : Exception {
			public readonly string Error;

			public TokenException(string error, string error_description, Exception innerException) : base(error_description, innerException) {
				Error = error;
			}
		}
        
        private async Task GetNewAccessToken() {
            var req = WebRequest.CreateHttp("https://beta.furrynetwork.com/api/oauth/token");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "application/json";
            req.UserAgent = "FurryNetworkLib/0.1 (https://www.github.com/libertyernie/FurryNetworkLib)";
            using (var sw = new StreamWriter(await req.GetRequestStreamAsync())) {
                await sw.WriteAsync($"client_id=123&");
                await sw.WriteAsync($"grant_type=refresh_token&");
                await sw.WriteAsync($"refresh_token={RefreshToken}");
                await sw.FlushAsync();
            }
			try {
				using (var resp = await req.GetResponseAsync())
				using (var sr = new StreamReader(resp.GetResponseStream())) {
					string json = await sr.ReadToEndAsync();
					var obj = JsonConvert.DeserializeAnonymousType(json, new {
						access_token = "",
						expires_in = 0,
						token_type = "",
						refresh_token = "",
						user_id = 0
					});
					if (obj.token_type != "bearer") {
						throw new Exception("Token returned was not a bearer token");
					}

					RefreshToken = obj.refresh_token ?? RefreshToken;
					AccessToken = obj.access_token;
				}
			} catch (WebException ex) {
				// Attempt to get information about the error.
				using (var sr = new StreamReader(ex.Response.GetResponseStream())) {
					string json = await sr.ReadToEndAsync();
					var o = JsonConvert.DeserializeAnonymousType(json, new {
						error = "",
						error_description = ""
					});
					throw new TokenException(o.error, o.error_description, ex);
				}
			}
        }

        /// <summary>
        /// Get information about the currently logged in user.
        /// </summary>
        public async Task<User> GetUserAsync() {
            using (var resp = await ExecuteRequest("GET", "user"))
            using (var sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<User>(json);
            }
        }

        /// <summary>
        /// Get information about the character with the given name.
        /// </summary>
        /// <param name="name">The character name</param>
        public async Task<Character> GetCharacterAsync(string name) {
            using (var resp = await ExecuteRequest("GET", $"character/{WebUtility.UrlEncode(name)}"))
            using (var sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Character>(json);
            }
        }

        /// <summary>
        /// Get information about a public artwork.
        /// </summary>
        /// <param name="id">The artwork ID</param>
        public async Task<Artwork> GetArtworkAsync(int id) {
            using (var resp = await ExecuteRequest("GET", $"artwork/{id}"))
            using (var sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Artwork>(json);
            }
		}

		/// <summary>
		/// Get information about a private artwork.
		/// </summary>
		/// <param name="id">The artwork ID</param>
		public async Task<Artwork> GetPrivateArtworkAsync(int id) {
			using (var resp = await ExecuteRequest("GET", $"artwork/{id}"))
			using (var sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<Artwork>(json);
			}
		}

		public class UpdateArtworkParameters {
			public IEnumerable<object> Collections { get; set; }
			public bool Community_tags_allowed { get; set; }
			public string Description { get; set; }
			public bool Publish { get; set; }
			public int Rating { get; set; } // 0, 1, 2
			public string Status { get; set; } // draft, unlisted, public
			public IEnumerable<string> Tags { get; set; }
			public string Title { get; set; }

			public UpdateArtworkParameters() {
				Collections = Enumerable.Empty<object>();
				Community_tags_allowed = true;
				Status = "draft";
				Tags = Enumerable.Empty<string>();
			}
		}

		public async Task UpdateArtwork(int id, UpdateArtworkParameters parameters) {
			using (var resp = await ExecuteRequest("PATCH", $"artwork/{id}", parameters))
			using (var sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				// Not trying to parse JSON because "tags" object is incorrectly a regular object and not an array
			}
		}

		public async Task DeleteArtwork(int id) {
			using (var resp = await ExecuteRequest("DELETE", $"artwork/{id}")) { }
		}

		/// <summary>
		/// Search submissions by type.
		/// </summary>
		/// <param name="type">The type (e.g. "artwork")</param>
		/// <param name="sort">The sort order</param>
		/// <param name="tags">A list of tags to filter by</param>
		/// <param name="from">The offset at which to start the search results</param>
		public async Task<SearchResults> SearchByTypeAsync(string type, string sort = null, IEnumerable<string> tags = null, int? from = 0) {
            string qs = "";
            if (!string.IsNullOrEmpty(sort)) {
                qs += $"&sort={WebUtility.UrlEncode(sort)}";
            }
			foreach (var tag in tags ?? Enumerable.Empty<string>()) {
				qs += $"&tags[]={WebUtility.UrlEncode(tag)}";
			}
            if (from != null) {
                qs += $"&from={from}";
            }
            using (var resp = await ExecuteRequest("GET", $"search/{WebUtility.UrlEncode(type)}?{qs}"))
            using (var sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<SearchResults>(json);
            }
        }

        /// <summary>
        /// Search submissions by the character under whose name it was uploaded.
        /// </summary>
        /// <param name="character">The character name</param>
        /// <param name="types">Filter to certain types (e.g. "artwork")</param>
        /// <param name="sort">The sort order</param>
        /// <param name="from">The offset at which to start the search results</param>
        public async Task<SearchResults> SearchByCharacterAsync(string character, IEnumerable<string> types = null, string sort = null, int? from = 0) {
            string qs = $"character={character}";
            if (types != null && types.Any()) {
                qs += $"&types[]={string.Join(",", types.Select(s => WebUtility.UrlEncode(s)))}";
            }
            if (!string.IsNullOrEmpty(sort)) {
                qs += $"&sort={WebUtility.UrlEncode(sort)}";
            }
            if (from != null) {
                qs += $"&from={from}";
            }
            using (var resp = await ExecuteRequest("GET", $"search?{qs}"))
            using (var sr = new StreamReader(resp.GetResponseStream())) {
                string json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<SearchResults>(json);
            }
		}

		/// <summary>
		/// Get a list of journals uploaded by a user/character.
		/// </summary>
		/// <param name="character">The character name</param>
		/// <param name="page">The page at which to start the search results</param>
		/// <param name="status">The status of journals to get (public, unlisted, or draft)</param>
		public async Task<JournalsResult> GetJournalsAsync(string character, int? page = 1, string status = "public") {
			using (var resp = await ExecuteRequest("GET", $"submission/lizard-socks/journal?page={page}&status={WebUtility.UrlEncode(status)}"))
			using (var sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<JournalsResult>(json);
			}
		}

		private const int ChunkSize = 524288;

		public async Task<Artwork> UploadArtwork(string characterName, byte[] data, string contentType, string filename) {
			string identifier = Guid.NewGuid().ToString();

			IEnumerable<byte[]> chunkGenerator() {
				for (int i = 0; i < data.Length; i += ChunkSize) {
					yield return data.Skip(i).Take(ChunkSize).ToArray();
				}
			};

			byte[][] chunks = chunkGenerator().ToArray();

			int chunkNumber = 1;
			foreach (byte[] partial in chunks) {
				string url = $"submission/{WebUtility.UrlEncode(characterName)}/artwork/upload?";
				url += $"resumableChunkNumber={chunkNumber}&";
				url += $"resumableChunkSize={ChunkSize}&";
				url += $"resumableCurrentChunkSize={partial.Length}&";
				url += $"resumableTotalSize={data.Length}&";
				url += $"resumableType={WebUtility.UrlEncode(contentType)}& ";
				url += $"resumableIdentifier={identifier}& ";
				url += $"resumableFilename={WebUtility.UrlEncode(filename)}&";
				url += $"resumableRelativePath={WebUtility.UrlEncode(filename)}&";
				url += $"resumableTotalChunks={chunks.Length}";

				using (var resp1 = await ExecuteRequest("GET", url)) { }

				var req2 = WebRequest.CreateHttp("https://beta.furrynetwork.com/api/" + url);
				req2.Method = "POST";
				req2.UserAgent = "FurryNetworkLib/0.1 (https://www.github.com/libertyernie/FurryNetworkLib)";
				if (AccessToken != null) {
					req2.Headers["Authorization"] = $"Bearer {AccessToken}";
				}
				req2.Accept = "application/json";
				req2.ContentType = "binary/octet-stream";
				req2.ContentLength = partial.Length;
				using (var stream = await req2.GetRequestStreamAsync()) {
					await stream.WriteAsync(partial, 0, partial.Length);
				}

				using (var resp2 = await req2.GetResponseAsync())
				using (var sr = new StreamReader(resp2.GetResponseStream())) {
					string body = await sr.ReadToEndAsync();
					if (resp2.ContentType == "application/json") {
						try {
							return JsonConvert.DeserializeObject<Artwork>(body);
						} catch (JsonException) { }
					}
				}

				chunkNumber++;
			}

			throw new Exception("No well-formed json response recieved");
		}

		/// <summary>
		/// Change the current character.
		/// </summary>
		/// <param name="newCharacterName">The character name to switch to</param>
		/// <returns>The newly selected character</returns>
		public async Task<Character> ChangeCharacterAsync(string newCharacterName) {
			using (var resp = await ExecuteRequest("POST", "current-character", new { character = newCharacterName }))
			using (var sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<Character>(json);
			}
		}

		/// <summary>
		/// Posts a new journal entry to Furry Network.
		/// </summary>
		/// <param name="characterName">Character name under which to post the journal</param>
		/// <param name="j">Journal entry data</param>
		/// <returns>The newly created journal entry</returns>
		public async Task<Journal> PostJournalAsync(string characterName, NewJournal j) {
			using (var resp = await ExecuteRequest("POST", $"journal", j))
			using (var sr = new StreamReader(resp.GetResponseStream())) {
				string json = await sr.ReadToEndAsync();
				return JsonConvert.DeserializeObject<Journal>(json);
			}
		}

		/// <summary>
		/// Deletes a journal entry from Furry Network.
		/// </summary>
		/// <param name="characterName">Character name that the journal is under</param>
		/// <param name="id">Journal ID</param>
		public async Task DeleteJournalAsync(string characterName, int id) {
			using (var resp = await ExecuteRequest("DELETE", $"journal/{id}"))
			using (var stream = resp.GetResponseStream()) {}
		}

		/// <summary>
		/// Invalidate the refresh token.
		/// </summary>
		public async Task LogoutAsync() {
            using (var resp = await ExecuteRequest("POST", "oauth/logout", jsonBody: new {
                refresh_token = RefreshToken
            })) { }

            AccessToken = null;
            RefreshToken = null;
        }
    }
}
