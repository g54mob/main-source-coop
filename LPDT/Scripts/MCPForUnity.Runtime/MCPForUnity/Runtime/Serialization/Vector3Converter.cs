using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class Vector3Converter : JsonConverter<Vector3>
	{
		public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(value.x);
			writer.WritePropertyName("y");
			writer.WriteValue(value.y);
			writer.WritePropertyName("z");
			writer.WriteValue(value.z);
			writer.WriteEndObject();
		}

		public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);
			if (jToken is JArray { Count: >=3 } jArray)
			{
				return new Vector3((float)jArray[0], (float)jArray[1], (float)jArray[2]);
			}
			if (!(jToken is JObject jObject))
			{
				throw new JsonSerializationException($"Cannot deserialize Vector3 from {jToken.Type}: '{jToken}'");
			}
			return new Vector3((float)jObject["x"], (float)jObject["y"], (float)jObject["z"]);
		}
	}
}
