using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSync {
	public class Settings {
		public class WeasylSettings {
			public string Username { get; set; }
			public string APIKey { get; set; }
		}

		public WeasylSettings Weasyl { get; set; }

		public class TumblrSettings {
			public string BlogName { get; set; }
			public string Footer { get; set; }
			public bool FooterIsLink { get; set; }
			public string Tags { get; set; }

			public string TokenKey { get; set; }
			public string TokenSecret { get; set; }
		}

		public TumblrSettings Tumblr { get; set; }

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

		public static Settings Load(string filename = "WeasylSync.json") {
			if (File.Exists(filename)) {
				Settings s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
				return s;
			} else {
				Settings s = new Settings {
					Weasyl = new WeasylSettings {
						Username = "example",
						APIKey = null,
					},
					Tumblr = new TumblrSettings {
						BlogName = "example",
						Footer = "<a href=\"{URL}\">View on Weasyl</a>",
						FooterIsLink = true,
						Tags = "#art",
						TokenKey = null,
						TokenSecret = null
					}
				};
				return s;
			}
		}

		public void Save(string filename = "WeasylSync.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(this, Formatting.Indented));
		}

		public Settings Copy() {
			return JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(this));
		}
	}
}
