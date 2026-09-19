using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class ColorConverter : JsonConverter<Color>
	{
		public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("r");
			writer.WriteValue(value.r);
			writer.WritePropertyName("g");
			writer.WriteValue(value.g);
			writer.WritePropertyName("b");
			writer.WriteValue(value.b);
			writer.WritePropertyName("a");
			writer.WriteValue(value.a);
			writer.WriteEndObject();
		}

		public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			return new Color((float)jObject["r"], (float)jObject["g"], (float)jObject["b"], (float)jObject["a"]);
		}
	}
}
