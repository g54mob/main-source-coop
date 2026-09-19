using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class QuaternionConverter : JsonConverter<Quaternion>
	{
		public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(value.x);
			writer.WritePropertyName("y");
			writer.WriteValue(value.y);
			writer.WritePropertyName("z");
			writer.WriteValue(value.z);
			writer.WritePropertyName("w");
			writer.WriteValue(value.w);
			writer.WriteEndObject();
		}

		public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);
			if (jToken is JArray { Count: >=4 } jArray)
			{
				return new Quaternion((float)jArray[0], (float)jArray[1], (float)jArray[2], (float)jArray[3]);
			}
			if (!(jToken is JObject jObject))
			{
				throw new JsonSerializationException($"Cannot deserialize Quaternion from {jToken.Type}: '{jToken}'");
			}
			return new Quaternion((float)jObject["x"], (float)jObject["y"], (float)jObject["z"], (float)jObject["w"]);
		}
	}
}
