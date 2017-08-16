using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace ArtSync {
	public class Settings {
		public class WeasylSettings {
			public string APIKey { get; set; }
		}

		public WeasylSettings Weasyl { get; set; }

        public class DeviantArtSettings {
            public string RefreshToken { get; set; }
        }

        public DeviantArtSettings DeviantArt { get; set; }

        public class TumblrSettings {
			public string BlogName { get; set; }

			public string TokenKey { get; set; }
			public string TokenSecret { get; set; }

			public bool AutoSidePadding { get; set; }
		}

		public TumblrSettings Tumblr { get; set; }

		public class TwitterSettings {
			public string TokenKey { get; set; }
			public string TokenSecret { get; set; }
		}

		public TwitterSettings Twitter { get; set; }

		public class InkbunnySettings {
			public string Sid { get; set; }
            public int? UserId { get; set; }
		}

		public InkbunnySettings Inkbunny { get; set; }

		public class PostSettings {
			public string HeaderHTML { get; set; }
			public string FooterHTML { get; set; }
			public string Tags { get; set; }
		}

		public PostSettings Defaults;

        public bool IncludeGeneratedUniqueTag { get; set; } = true;

		[JsonIgnore]
		public Token TumblrToken {
			get {
				return new Token(Tumblr.TokenKey, Tumblr.TokenSecret);
			}
			set {
				Tumblr.TokenKey = value == null ? null : value.Key;
				Tumblr.TokenSecret = value == null ? null : value.Secret;
			}
		}

		[JsonIgnore]
		public TwitterCredentials TwitterCredentials {
			get {
				return Twitter?.TokenKey == null || Twitter?.TokenSecret == null
					? null
					: new TwitterCredentials(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET, Twitter.TokenKey, Twitter.TokenSecret);
			}
			set {
				Twitter.TokenKey = value?.AccessToken;
				Twitter.TokenSecret = value?.AccessTokenSecret;
			}
		}

		public static Settings Load(string filename = "ArtSync.json") {
			Settings s = new Settings();
			if (filename != null && File.Exists(filename)) {
				s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
			}
			if (s.Weasyl == null)
				s.Weasyl = new WeasylSettings {
					APIKey = null
				};
            if (s.DeviantArt == null)
                s.DeviantArt = new DeviantArtSettings {
                    RefreshToken = null
                };
            if (s.Tumblr == null)
				s.Tumblr = new TumblrSettings {
					BlogName = "",
					TokenKey = null,
					TokenSecret = null,
					AutoSidePadding = true
				};
			if (s.Twitter == null)
				s.Twitter = new TwitterSettings {
					TokenKey = null,
					TokenSecret = null
				};
			if (s.Inkbunny == null) {
				s.Inkbunny = new InkbunnySettings {
                    Sid = null
				};
			}
			if (s.Defaults == null)
				s.Defaults = new PostSettings {
					HeaderHTML = "<p><b>{TITLE}</b></p>",
					FooterHTML = "<p><a href=\"{URL}\">View on {SITENAME}</a></p>",
					Tags = "#art",
				};
			return s;
		}

		public void Save(string filename = "ArtSync.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
		}

		public Settings Copy() {
			return JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(this));
		}
	}
}
