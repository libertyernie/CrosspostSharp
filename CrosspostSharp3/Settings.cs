using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public class Settings {
		public abstract class AccountCredentials {
			public string Username { get; set; }
		}

		public class DeviantArtSettings {
			public string RefreshToken { get; set; }
		}

		public DeviantArtSettings DeviantArt { get; set; }

		public class FurAffinitySettings : AccountCredentials {
			public string b;
			public string a;
		}

		public List<FurAffinitySettings> FurAffinity = new List<FurAffinitySettings>();

		public class TwitterSettings : AccountCredentials {
			public string tokenKey;
			public string tokenSecret;

			//string ITwitterCredentials.AccessToken { get => tokenKey; set => throw new NotImplementedException(); }
			//string ITwitterCredentials.AccessTokenSecret { get => tokenSecret; set => throw new NotImplementedException(); }
			//string IConsumerCredentials.ConsumerKey { get => OAuthConsumer.Twitter.CONSUMER_KEY; set => throw new NotImplementedException(); }
			//string IConsumerCredentials.ConsumerSecret { get => OAuthConsumer.Twitter.CONSUMER_SECRET; set => throw new NotImplementedException(); }
			//string IConsumerCredentials.ApplicationOnlyBearerToken { get => null; set => throw new NotImplementedException(); }

			//bool IConsumerCredentials.AreSetupForApplicationAuthentication() {
			//	return true;
			//}

			//bool ITwitterCredentials.AreSetupForUserAuthentication() {
			//	return true;
			//}

			public ITwitterCredentials Clone() {
				return new TwitterCredentials(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET, tokenKey, tokenSecret);
			}

			//IConsumerCredentials IConsumerCredentials.Clone() {
			//	return new ConsumerCredentials(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET);
			//}
		}

		public List<TwitterSettings> Twitter = new List<TwitterSettings>();

		public static Settings Load(string filename = "CrosspostSharp.json") {
			Settings s = new Settings();
			if (filename != null && File.Exists(filename)) {
				s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
			}
			return s;
		}

		public void Save(string filename = "CrosspostSharp.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
	}
}
