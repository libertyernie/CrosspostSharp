using FlickrNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public class Settings {
		public interface IAccountCredentials {
			string Username { get; }
		}

		public struct DeviantArtSettings {
			public string RefreshToken { get; set; }
		}

		public DeviantArtSettings DeviantArt { get; set; }

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
			public string wzl;

			string IAccountCredentials.Username => username;
		}

		public List<WeasylSettings> Weasyl = new List<WeasylSettings>();

		public static Settings Load(string filename = "CrosspostSharp3.json") {
			Settings s = new Settings();
			if (filename != null && File.Exists(filename)) {
				s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
			}
			return s;
		}

		public void Save(string filename = "CrosspostSharp3.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
	}
}
