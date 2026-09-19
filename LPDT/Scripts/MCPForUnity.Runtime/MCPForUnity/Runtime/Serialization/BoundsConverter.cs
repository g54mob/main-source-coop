using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class BoundsConverter : JsonConverter<Bounds>
	{
		public override void WriteJson(JsonWriter writer, Bounds value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("center");
			serializer.Serialize(writer, value.center);
			writer.WritePropertyName("size");
			serializer.Serialize(writer, value.size);
			writer.WriteEndObject();
		}

		public override Bounds ReadJson(JsonReader reader, Type objectType, Bounds existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			Vector3 center = jObject["center"].ToObject<Vector3>(serializer);
			Vector3 size = jObject["size"].ToObject<Vector3>(serializer);
			return new Bounds(center, size);
		}
	}
}
