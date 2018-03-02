using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrosspostSharp3 {
	public struct Settings {
		public struct FurAffinitySettings {
			public string b { get; set; }
			public string a { get; set; }
		}

		public FurAffinitySettings FurAffinity;

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
