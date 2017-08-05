using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
	[JsonConverter(typeof(InkbunnyTFBooleanConverter))]
	public struct InkbunnyTFBoolean {
		public bool value;

		public static implicit operator bool(InkbunnyTFBoolean i) {
			return i.value;
		}
	}

	public class InkbunnyTFBooleanConverter : JsonConverter {
		public override bool CanConvert(Type objectType) {
			return objectType == typeof(InkbunnyTFBoolean);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			string v = reader.Value?.ToString();
			if (v == "t") return new InkbunnyTFBoolean { value = true };
			if (v == "f") return new InkbunnyTFBoolean { value = false };
			throw new JsonReaderException("Expected value of 't' or 'f' for InkbunnyTFBoolean. Path: " + reader.Path);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			var i = (InkbunnyTFBoolean)value;
			writer.WriteValue(i.value ? "t" : "f");
		}
	}
}
