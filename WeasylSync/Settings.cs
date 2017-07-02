using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace WeasylSync {
	public class Settings {
		public int SettingsVersion {
			get {
				return 1;
			}
			set {
				// ignore
			}
		}

		public class WeasylSettings {
			public string APIKey { get; set; }
		}

		public WeasylSettings Weasyl { get; set; }

		public class TumblrSettings {
			public string BlogName { get; set; }

			public string TokenKey { get; set; }
			public string TokenSecret { get; set; }

			public bool AutoSidePadding { get; set; }
			public bool FindPreviousPost { get; set; }
		}

		public TumblrSettings Tumblr { get; set; }

        public class TwitterSettings {
            public string TokenKey { get; set; }
            public string TokenSecret { get; set; }
        }

        public TwitterSettings Twitter { get; set; }

        public class InkbunnySettings {
            public string DefaultUsername { get; set; }
            public string DefaultPassword { get; set; }
        }

        public InkbunnySettings Inkbunny { get; set; }

        public class PostSettings {
			public string HeaderHTML { get; set; }
			public string FooterHTML { get; set; }
			public string Tags { get; set; }

			public bool IncludeWeasylTag { get; set; }
		}

		public PostSettings Defaults;

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
                return new TwitterCredentials(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET, Twitter?.TokenKey, Twitter?.TokenSecret);
            }
            set {
                Twitter.TokenKey = value?.AccessToken;
                Twitter.TokenSecret = value?.AccessTokenSecret;
            }
        }

        public static Settings Load(string filename = "WeasylSync.json") {
			Settings s = new Settings();
			if (filename != null && File.Exists(filename)) {
				s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
			}
			s.SettingsVersion = 1;
			if (s.Weasyl == null)
				s.Weasyl = new WeasylSettings {
					APIKey = null
				};
			if (s.Tumblr == null)
				s.Tumblr = new TumblrSettings {
					BlogName = "",
					TokenKey = null,
					TokenSecret = null,
					AutoSidePadding = true,
					FindPreviousPost = true
				};
            if (s.Twitter == null)
                s.Twitter = new TwitterSettings {
                    TokenKey = null,
                    TokenSecret = null
                };
            if (s.Inkbunny == null) {
                s.Inkbunny = new InkbunnySettings {
                    DefaultUsername = "",
                    DefaultPassword = ""
                };
            }
			if (s.Defaults == null)
				s.Defaults = new PostSettings {
					HeaderHTML = "<p><b>{TITLE}</b></p>",
					FooterHTML = "<p><a href=\"{URL}\">View on Weasyl</a></p>",
					Tags = "#art",
					IncludeWeasylTag = true
				};
			return s;
		}

		public void Save(string filename = "WeasylSync.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
		}

		public Settings Copy() {
			return JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(this));
		}
	}
}
