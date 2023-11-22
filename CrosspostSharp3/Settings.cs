using CrosspostSharp3.DeviantArt;
using Newtonsoft.Json;
using Pleronet;
using Pleronet.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrosspostSharp3 {
	public class Settings {
		public interface IAccountCredentials {
			string Username { get; }
		}

		public class DeviantArtAccountSettings : IAccountCredentials {
			public string AccessToken { get; set; }
			public string RefreshToken { get; set; }
			public string Username { get; set; }
		}

		public List<DeviantArtAccountSettings> DeviantArtAccounts = new();

		public IEnumerable<DeviantArtTokenWrapper> DeviantArtTokens =>
			DeviantArtAccounts.Select(x => new DeviantArtTokenWrapper(this, x));

		public struct FurAffinitySettings : IAccountCredentials, FurAffinityFs.FurAffinity.ICredentials {
			public string b;
			public string a;
			public string username;

			readonly string IAccountCredentials.Username => username;

			readonly string FurAffinityFs.FurAffinity.ICredentials.A => a;
			readonly string FurAffinityFs.FurAffinity.ICredentials.B => b;
		}

		public List<FurAffinitySettings> FurAffinity = new();

		public struct FurryNetworkSettings : IAccountCredentials {
			public string refreshToken;
			public string characterName;

			readonly string IAccountCredentials.Username => characterName;
		}

		public List<FurryNetworkSettings> FurryNetwork = new();

		public struct InkbunnySettings : IAccountCredentials {
			public string sid;
			public int userId;
			public string username;

			readonly string IAccountCredentials.Username => username;
		}

		public List<InkbunnySettings> Inkbunny = new();

		public List<PleronetSettings> Pleronet = new();

		public struct PleronetSettings : IAccountCredentials {
			public AppRegistration AppRegistration { get; set; }
			public Auth Auth { get; set; }
			public string Username { get; set; }

			public readonly MastodonClient GetClient() {
				return new MastodonClient(AppRegistration, Auth);
			}
		}

		public List<PleronetSettings> Pixelfed = new();

		public struct TumblrSettings : IAccountCredentials {
			public string tokenKey;
			public string tokenSecret;
			public string blogName;
			public IEnumerable<string> tags;

			readonly string IAccountCredentials.Username => blogName;
		}

		public List<TumblrSettings> Tumblr = new();

		public struct WeasylSettings : IAccountCredentials {
			public string username;
			public string apiKey;

			readonly string IAccountCredentials.Username => username;
		}

		public List<WeasylSettings> WeasylApi = new();

		public static Settings Load(string filename = "CrosspostSharp3.json") {
			Settings s = new();
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
