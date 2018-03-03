using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrosspostSharp3 {
	public class Settings {
		public class DeviantArtSettings {
			public string RefreshToken { get; set; }
		}

		public DeviantArtSettings DeviantArt { get; set; }

		public class FurAffinitySettings {
			public string username;
			public string b;
			public string a;

			public override string ToString() {
				return username;
			}
		}

		public List<FurAffinitySettings> FurAffinity = new List<FurAffinitySettings>();

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
