using ArtworkSourceSpecification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrosspostSharp3.FurAffinity {
	public class FAExportArtworkSource : IArtworkSource {
		private readonly string _fa_cookie;
		private readonly bool _sfw;
		private readonly string _folder;

		public FAExportArtworkSource(string fa_cookie, bool sfw, string folder) {
			_fa_cookie = fa_cookie ?? throw new ArgumentNullException(nameof(fa_cookie));
			_sfw = sfw;
			_folder = folder;
		}

		public const string FAExportHost = "faexport.spangle.org.uk";

		public string Name => $"FurAffinity ({_folder})";

		public record CurrentUser {
			public string profile_name;
		}

		public record Notifications {
			public CurrentUser current_user;
		}

		public static async Task<Notifications> GetNotificationsAsync(string fa_cookie, bool sfw, int from) {
			var req = WebRequest.CreateHttp($"https://{FAExportHost}/notifications/others.json?{(sfw ? "sfw=1" : "")}&from={from}");
			req.UserAgent = "CrosspostSharp/4.0 (https://github.com/libertyernie/CrosspostSharp)";
			req.Headers.Set("FA_COOKIE", fa_cookie);
			using var resp = await req.GetResponseAsync();
			using var stream = resp.GetResponseStream();
			using var sr = new StreamReader(stream);
			string json = await sr.ReadToEndAsync();
			return JsonConvert.DeserializeObject<Notifications>(json);
		}

		public static async Task<string> GetUsernameAsync(string fa_cookie, bool sfw) {
			var resp = await GetNotificationsAsync(fa_cookie, sfw, from: int.MaxValue);
			return resp.current_user.profile_name;
		}

		public record Profile : IAuthor {
			public string name;
			public string profile;
			public string avatar;

			string IAuthor.Name => name;
			string IAuthor.IconUrl => avatar;
		}

		public static async Task<Profile> GetProfileAsync(string fa_cookie, bool sfw, string username) {
			var req = WebRequest.CreateHttp($"https://{FAExportHost}/user/{username}.json?{(sfw ? "sfw=1" : "")}");
			req.UserAgent = "CrosspostSharp/4.0 (https://github.com/libertyernie/CrosspostSharp)";
			req.Headers.Set("FA_COOKIE", fa_cookie);
			using var resp = await req.GetResponseAsync();
			using var stream = resp.GetResponseStream();
			using var sr = new StreamReader(stream);
			string json = await sr.ReadToEndAsync();
			return JsonConvert.DeserializeObject<Profile>(json);
		}

		public static async Task<Profile> GetCurrentProfileAsync(string fa_cookie, bool sfw) {
			var username = await GetUsernameAsync(fa_cookie, sfw);
			return await GetProfileAsync(fa_cookie, sfw, username);
		}

		public static async Task<ImmutableList<int>> GetFolderAsync(string fa_cookie, bool sfw, string username, string folder, int page = 1) {
			var req = WebRequest.CreateHttp($"https://{FAExportHost}/user/{Uri.EscapeDataString(username)}/{Uri.EscapeDataString(folder)}.json?{(sfw ? "sfw=1" : "")}&page={page}");
			req.UserAgent = "CrosspostSharp/4.0 (https://github.com/libertyernie/CrosspostSharp)";
			req.Headers.Set("FA_COOKIE", fa_cookie);
			using var resp = await req.GetResponseAsync();
			using var stream = resp.GetResponseStream();
			using var sr = new StreamReader(stream);
			string json = await sr.ReadToEndAsync();
			return JsonConvert.DeserializeObject<ImmutableList<int>>(json);
		}

		public static async IAsyncEnumerable<int> GetFolderAsAsyncEnumerable(string fa_cookie, bool sfw, string username, string folder) {
			int page = 1;
			while (true) {
				var result = await GetFolderAsync(fa_cookie, sfw, username, folder, page);
				foreach (int id in result)
					yield return id;
				page += 1;
			}
		}

		public record FullSubmission : IRemotePhotoPost {
			public string title;
			public string description;
			public string link;
			public DateTimeOffset posted_at;
			public string download;
			public string full;
			public string thumbnail;
			public string rating;
			public ImmutableList<string> keywords;

			string IRemotePhotoPost.ImageURL => download ?? full;
			string IThumbnailPost.ThumbnailURL => thumbnail ?? download ?? full;
			string IPostBase.Title => title;
			string IPostBase.HTMLDescription => description;
			bool IPostBase.Mature => rating != "General";
			bool IPostBase.Adult => rating != "General" && rating != "Mature";
			IEnumerable<string> IPostBase.Tags => keywords;
			DateTime IPostBase.Timestamp => posted_at.UtcDateTime;
			string IPostBase.ViewURL => link;
		}

		public static async Task<FullSubmission> GetSubmissionAsync(string fa_cookie, bool sfw, int id) {
			var req = WebRequest.CreateHttp($"https://{FAExportHost}/submission/{id}.json?{(sfw ? "sfw=1" : "")}");
			req.UserAgent = "CrosspostSharp/4.0 (https://github.com/libertyernie/CrosspostSharp)";
			req.Headers.Set("FA_COOKIE", fa_cookie);
			using var resp = await req.GetResponseAsync();
			using var stream = resp.GetResponseStream();
			using var sr = new StreamReader(stream);
			string json = await sr.ReadToEndAsync();
			return JsonConvert.DeserializeObject<FullSubmission>(json);
		}

		public async Task<IAuthor> GetUserAsync() {
			return await GetCurrentProfileAsync(_fa_cookie, _sfw);
		}

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var username = await GetUsernameAsync(_fa_cookie, _sfw);
			await foreach (int id in GetFolderAsAsyncEnumerable(_fa_cookie, _sfw, username, _folder)) {
				yield return await GetSubmissionAsync(_fa_cookie, _sfw, id);
			}
		}
	}
}
