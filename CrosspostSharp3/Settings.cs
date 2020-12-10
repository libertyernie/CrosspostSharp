using CrosspostSharp3.DeviantArt;
using FurAffinityFs;
using MapleFedNet.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

		public class DeviantArtAccountSettings : IAccountCredentials {
			public string AccessToken { get; set; }
			public string RefreshToken { get; set; }
			public string Username { get; set; }
		}

		public List<DeviantArtAccountSettings> DeviantArtAccounts = new List<DeviantArtAccountSettings>();

		public IEnumerable<DeviantArtTokenWrapper> DeviantArtTokens =>
			DeviantArtAccounts.Select(x => new DeviantArtTokenWrapper(this, x));

		[DefaultValue(true), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public bool DeviantArtEclipse = true;

		public struct FurAffinitySettings : IAccountCredentials, IFurAffinityCredentials {
			public string b;
			public string a;
			public string username;

			string IAccountCredentials.Username => username;

			string IFurAffinityCredentials.A => a;
			string IFurAffinityCredentials.B => b;
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

		public List<MastodonSettings> Mastodon = new List<MastodonSettings>();

		public struct MastodonSettings : IAccountCredentials, IMastodonCredentials {
			public string Instance;
			public string accessToken;
			public string username;

			string IAccountCredentials.Username => username;

			string IMastodonCredentials.Domain => Instance;
			string IMastodonCredentials.Token => accessToken;
		}

		public struct PinterestSettings : IAccountCredentials {
			public string accessToken;
			public string username;
			public string boardName;

			string IAccountCredentials.Username => boardName;
		}

		public struct PixivUploadSettings : PixivUploader.IPixivSession, IAccountCredentials {
			public string PHPSESSID { get; set; }

			string IAccountCredentials.Username => PHPSESSID;
		}

		public List<PixivUploadSettings> PixivUpload = new List<PixivUploadSettings>();

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

		public void Save() {
			File.WriteAllText("CrosspostSharp3.json", JsonConvert.SerializeObject(this, Formatting.Indented));
		}
	}
}
