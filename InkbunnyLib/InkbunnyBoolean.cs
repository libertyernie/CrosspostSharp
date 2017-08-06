using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
	[JsonConverter(typeof(InkbunnyTFBooleanConverter))]
	public struct InkbunnyBoolean {
		public bool value;

		public static implicit operator bool(InkbunnyBoolean i) {
			return i.value;
		}

		public override string ToString() {
			return value ? "t" : "f";
		}
	}

	public class InkbunnyTFBooleanConverter : JsonConverter {
		public override bool CanConvert(Type objectType) {
			return objectType == typeof(InkbunnyBoolean);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			string v = reader.Value?.ToString();
			if (v == "t") return new InkbunnyBoolean { value = true };
			if (v == "f") return new InkbunnyBoolean { value = false };
			throw new JsonReaderException("Expected value of 't' or 'f' for InkbunnyTFBoolean. Path: " + reader.Path);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			var i = (InkbunnyBoolean)value;
			writer.WriteValue(i.value ? "t" : "f");
		}
	}
}
