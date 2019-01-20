using DeviantArtFs;
using FlickrNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public class Settings {
		public struct MainFormSettings {
			public int rows, columns;
		}

		public MainFormSettings? MainForm { get; set; }

		public interface IAccountCredentials {
			string Username { get; }
		}

		[Obsolete]
		public struct DeviantArtSettings {
			public string RefreshToken { get; set; }
		}

		[Obsolete]
		public DeviantArtSettings? DeviantArt { get; set; }

		public struct DeviantArtAccountSettings : IAccountCredentials, IDeviantArtRefreshToken {
			public string AccessToken { get; set; }
			public DateTimeOffset ExpiresAt { get; set; }
			public string RefreshToken { get; set; }
			public string Username { get; set; }
		}

		public List<DeviantArtAccountSettings> DeviantArtAccounts = new List<DeviantArtAccountSettings>();

		public struct FlickrSettings : IAccountCredentials {
			public string tokenKey;
			public string tokenSecret;
			public string username;

			string IAccountCredentials.Username => username;

			public Flickr CreateClient() {
				return new Flickr(OAuthConsumer.Flickr.KEY, OAuthConsumer.Flickr.SECRET) {
					OAuthAccessToken = tokenKey,
					OAuthAccessTokenSecret = tokenSecret
				};
			}
		}

		public List<FlickrSettings> Flickr = new List<FlickrSettings>();

		public struct FurAffinitySettings : IAccountCredentials {
			public string b;
			public string a;
			public string username;

			string IAccountCredentials.Username => username;
		}

		public List<FurAffinitySettings> FurAffinity = new List<FurAffinitySettings>();

		public struct FurryNetworkSettings : IAccountCredentials {
			public string refreshToken;
			public string characterName;

			string IAccountCredentials.Username => characterName;
		}

		public List<FurryNetworkSettings> FurryNetwork = new List<FurryNetworkSettings>();

		public struct InkbunnySettings : IAccountCredentials {
			public string sid;
			public int userId;
			public string username;

			string IAccountCredentials.Username => username;
		}

		public List<InkbunnySettings> Inkbunny = new List<InkbunnySettings>();

		public struct PillowfortSettings : IAccountCredentials {
			public string username;
			public string cookie;
			public IEnumerable<string> tags;

			string IAccountCredentials.Username => username;
		}

		public List<MastodonSettings> Mastodon = new List<MastodonSettings>();

		public struct MastodonSettings : IAccountCredentials {
			public string instance;
			public string accessToken;
			public string username;

			string IAccountCredentials.Username => username;
		}

		public List<PillowfortSettings> Pillowfort = new List<PillowfortSettings>();

		public struct PinterestSettings : IAccountCredentials {
			public string accessToken;
			public string username;
			public string boardName;

			string IAccountCredentials.Username => boardName;
		}
		
		public struct PixivSettings : IAccountCredentials {
			public string username;
			public string password;

			string IAccountCredentials.Username => username;
		}

		public List<PixivSettings> Pixiv = new List<PixivSettings>();

		public struct TwitterSettings : IAccountCredentials {
			public string tokenKey;
			public string tokenSecret;
			public string screenName;

			string IAccountCredentials.Username => screenName;

			public ITwitterCredentials GetCredentials() {
				return new TwitterCredentials(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET, tokenKey, tokenSecret);
			}
		}

		public List<TwitterSettings> Twitter = new List<TwitterSettings>();

		public struct MediaRSSSettings : IAccountCredentials {
			public string url;
			public string name;

			string IAccountCredentials.Username => name;
		}

		public List<MediaRSSSettings> MediaRSS = new List<MediaRSSSettings>();

		public struct TumblrSettings : IAccountCredentials {
			public string tokenKey;
			public string tokenSecret;
			public string blogName;
			public IEnumerable<string> tags;

			string IAccountCredentials.Username => blogName;
		}

		public List<TumblrSettings> Tumblr = new List<TumblrSettings>();

		public struct WeasylSettings : IAccountCredentials {
			public string username;
			public string apiKey;

			string IAccountCredentials.Username => username;
		}

		public List<WeasylSettings> WeasylApi = new List<WeasylSettings>();

		public static Settings Load(string filename = "CrosspostSharp3.json") {
			Settings s = new Settings();
			if (filename != null && File.Exists(filename)) {
				s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
			}
			return s;
		}

		public async Task<bool> UpdateTokensAsync() {
			bool changed = false;
			if (DeviantArt?.RefreshToken != null) {
				DeviantArtAccounts.Add(new DeviantArtAccountSettings {
					AccessToken = "",
					ExpiresAt = DateTimeOffset.UtcNow,
					RefreshToken = DeviantArt?.RefreshToken,
					Username = ""
				});
				DeviantArt = null;
			}
			foreach (var da in DeviantArtAccounts.ToArray()) {
				if (DateTime.UtcNow > da.ExpiresAt.AddMinutes(-15)) {
					// Get new access token
					var a = new DeviantArtAuth(OAuthConsumer.DeviantArt.CLIENT_ID, OAuthConsumer.DeviantArt.CLIENT_SECRET);
					var t = await a.RefreshAsync(da.RefreshToken);
					var u = await DeviantArtFs.Requests.User.Whoami.ExecuteAsync(t);
					DeviantArtAccounts.Remove(da);
					DeviantArtAccounts.Add(new DeviantArtAccountSettings {
						AccessToken = t.AccessToken,
						ExpiresAt = t.ExpiresAt,
						RefreshToken = t.RefreshToken,
						Username = u.Username
					});
					changed = true;
				}
			}
			return changed;
		}

		public void Save(string filename = "CrosspostSharp3.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
	}
}
