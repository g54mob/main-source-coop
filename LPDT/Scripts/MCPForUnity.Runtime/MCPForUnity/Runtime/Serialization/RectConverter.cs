using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class RectConverter : JsonConverter<Rect>
	{
		public override void WriteJson(JsonWriter writer, Rect value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(value.x);
			writer.WritePropertyName("y");
			writer.WriteValue(value.y);
			writer.WritePropertyName("width");
			writer.WriteValue(value.width);
			writer.WritePropertyName("height");
			writer.WriteValue(value.height);
			writer.WriteEndObject();
		}

		public override Rect ReadJson(JsonReader reader, Type objectType, Rect existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			return new Rect((float)jObject["x"], (float)jObject["y"], (float)jObject["width"], (float)jObject["height"]);
		}
	}
}
