using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSync {
	public class Settings {
		public string AccessTokenKey { get; set; }
		public string AccessTokenSecretEnc { get; set; }

		public static Settings Global;

		static Settings() {
			if (File.Exists("WeasylSyncSettings.json")) {
				Global = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("WeasylSyncSettings.json"));
			} else {
				Global = new Settings();
			}
		}

		public void Save(string filename = "WeasylSyncSettings.json") {
			File.WriteAllText(filename, JsonConvert.SerializeObject(Global));
		}
	}
}
