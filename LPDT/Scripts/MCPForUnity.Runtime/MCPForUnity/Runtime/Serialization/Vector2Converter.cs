using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class Vector2Converter : JsonConverter<Vector2>
	{
		public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(value.x);
			writer.WritePropertyName("y");
			writer.WriteValue(value.y);
			writer.WriteEndObject();
		}

		public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);
			if (jToken is JArray { Count: >=2 } jArray)
			{
				return new Vector2((float)jArray[0], (float)jArray[1]);
			}
			if (!(jToken is JObject jObject))
			{
				throw new JsonSerializationException($"Cannot deserialize Vector2 from {jToken.Type}: '{jToken}'");
			}
			return new Vector2((float)jObject["x"], (float)jObject["y"]);
		}
	}
}
