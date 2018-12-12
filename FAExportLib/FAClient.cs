using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FAExportLib {
	public class FAClient {
		private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings {
			ContractResolver = new DefaultContractResolver {
				NamingStrategy = new SnakeCaseNamingStrategy()
			}
		};

		/// <summary>
		/// Create a client that connects to FAExport without logging in as a particular user.
		/// This allows access to accounts and pictures hidden from guests, including mature and adult content, but does not allow posting journals.
		/// </summary>
		public FAClient() { }

		public string UserAgent { get; set; } = "FAClient/0.4 (https://github.com/libertyernie/CrosspostSharp)";

		protected virtual string GetFACookie() {
			return null;
		}

		private async Task<string> FAExportRequestAsync(string url, bool useCookie = true) {
			var request = WebRequest.CreateHttp(url);
			request.UserAgent = UserAgent;
			if (useCookie) {
				var cookie = GetFACookie();
				if (cookie != null)
					request.Headers.Add("FA_COOKIE", cookie);
			}
			try {
				using (var response = await request.GetResponseAsync())
				using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
					return await sr.ReadToEndAsync();
				}
			} catch (WebException ex) {
				throw new FAExportException(ex);
			}
		}

		/// <summary>
		/// Get information about a user.
		/// </summary>
		/// <param name="name">A FurAffinity username</param>
		public async Task<FAUser> GetUserAsync(string name) {
			var json = await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(name)}.json");
			return JsonConvert.DeserializeObject<FAUser>(json, _jsonSettings);
		}

		/// <summary>
		/// Get a list of submission IDs for a user's gallery, scraps, or favorites.
		/// </summary>
		/// <param name="username">A FurAffinity username</param>
		/// <param name="folder">The folder type (gallery, scraps, or favorites)</param>
		/// <param name="page">The page to start at (each page has up to 60 submissions)</param>
		public async Task<IEnumerable<int>> GetSubmissionIdsAsync(string username, FAFolder folder, int page = 1) {
			var json = await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?page={page}&perpage=60");
			return JsonConvert.DeserializeObject<IEnumerable<int>>(json, _jsonSettings);
		}

		/// <summary>
		/// Get a list of minimal submission data for a user's gallery, scraps, or favorites.
		/// </summary>
		/// <param name="username">A FurAffinity username</param>
		/// <param name="folder">The folder type (gallery, scraps, or favorites)</param>
		/// <param name="page">The page to start at (each page has up to 60 submissions)</param>
		public async Task<IEnumerable<FAFolderSubmission>> GetSubmissionsAsync(string username, FAFolder folder, int page = 1) {
			var json = await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?full=1&page={page}&perpage=60");
			return JsonConvert.DeserializeObject<IEnumerable<FAFolderSubmission>>(json);
		}

		/// <summary>
		/// Get a list of submission IDs from a search query.
		/// </summary>
		public async Task<IEnumerable<int>> SearchSubmissionIdsAsync(
			string q,
			int page = 1,
			FAOrder order_by = FAOrder.date,
			FAOrderDirection order_direction = FAOrderDirection.desc,
			FARange range = FARange.all,
			FASearchMode mode = FASearchMode.extended,
			FARating rating = FARating.general | FARating.mature | FARating.adult,
			FAType type = FAType.art | FAType.flash | FAType.music | FAType.photo | FAType.poetry | FAType.story
		) {
			var url = $"https://faexport.boothale.net/search.json?q={WebUtility.UrlEncode(q)}&page={page}&perpage=60&order_by={order_by}&order_direction={order_direction}&range={range}&mode={mode}&rating={rating.ToString().Replace(" ", "")}&type={type.ToString().Replace(" ", "")}";
			var json = await FAExportRequestAsync(url);
			return JsonConvert.DeserializeObject<IEnumerable<int>>(json, _jsonSettings);
		}

		/// <summary>
		/// Get a list of submissions from a search query.
		/// </summary>
		public async Task<IEnumerable<FAFolderSubmission>> SearchSubmissionsAsync(
			string q,
			int page = 1,
			FAOrder order_by = FAOrder.date,
			FAOrderDirection order_direction = FAOrderDirection.desc,
			FARange range = FARange.all,
			FASearchMode mode = FASearchMode.extended,
			FARating rating = FARating.general | FARating.mature | FARating.adult,
			FAType type = FAType.art | FAType.flash | FAType.music | FAType.photo | FAType.poetry | FAType.story
		) {
			var url = $"https://faexport.boothale.net/search.json?q={WebUtility.UrlEncode(q)}&page={page}&perpage=60&order_by={order_by}&order_direction={order_direction}&range={range}&mode={mode}&rating={rating.ToString().Replace(" ", "")}&type={type.ToString().Replace(" ", "")}&full=1";
			var json = await FAExportRequestAsync(url);
			return JsonConvert.DeserializeObject<IEnumerable<FAFolderSubmission>>(json, _jsonSettings);
		}

		/// <summary>
		/// Get in-depth data about a particular submission.
		/// </summary>
		/// <param name="id">The numeric submission ID</param>
		public async Task<FASubmission> GetSubmissionAsync(int id) {
			var json = await FAExportRequestAsync($"https://faexport.boothale.net/submission/{id}.json", useCookie: false);
			return JsonConvert.DeserializeObject<FASubmission>(json, _jsonSettings);
		}

		/// <summary>
		/// Get a list of journals posted by a user.
		/// </summary>
		/// <param name="username">A FurAffinity username</param>
		public async Task<IEnumerable<FAJournal>> GetJournalsAsync(string username) {
			var json = await FAExportRequestAsync($"https://faexport.boothale.net/user/{username}/journals.json?full=1");
			return JsonConvert.DeserializeObject<IEnumerable<FAJournal>>(json, _jsonSettings);
		}
	}
}
