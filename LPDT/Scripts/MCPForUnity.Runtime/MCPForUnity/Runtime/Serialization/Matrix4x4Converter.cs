using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	public class Matrix4x4Converter : JsonConverter<Matrix4x4>
	{
		public override void WriteJson(JsonWriter writer, Matrix4x4 value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("m00");
			writer.WriteValue(value.m00);
			writer.WritePropertyName("m01");
			writer.WriteValue(value.m01);
			writer.WritePropertyName("m02");
			writer.WriteValue(value.m02);
			writer.WritePropertyName("m03");
			writer.WriteValue(value.m03);
			writer.WritePropertyName("m10");
			writer.WriteValue(value.m10);
			writer.WritePropertyName("m11");
			writer.WriteValue(value.m11);
			writer.WritePropertyName("m12");
			writer.WriteValue(value.m12);
			writer.WritePropertyName("m13");
			writer.WriteValue(value.m13);
			writer.WritePropertyName("m20");
			writer.WriteValue(value.m20);
			writer.WritePropertyName("m21");
			writer.WriteValue(value.m21);
			writer.WritePropertyName("m22");
			writer.WriteValue(value.m22);
			writer.WritePropertyName("m23");
			writer.WriteValue(value.m23);
			writer.WritePropertyName("m30");
			writer.WriteValue(value.m30);
			writer.WritePropertyName("m31");
			writer.WriteValue(value.m31);
			writer.WritePropertyName("m32");
			writer.WriteValue(value.m32);
			writer.WritePropertyName("m33");
			writer.WriteValue(value.m33);
			writer.WriteEndObject();
		}

		public override Matrix4x4 ReadJson(JsonReader reader, Type objectType, Matrix4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return default(Matrix4x4);
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonSerializationException($"Expected JSON object or null when deserializing Matrix4x4, got '{reader.TokenType}'.");
			}
			JObject jObject = JObject.Load(reader);
			return new Matrix4x4
			{
				m00 = (jObject["m00"]?.Value<float>() ?? 0f),
				m01 = (jObject["m01"]?.Value<float>() ?? 0f),
				m02 = (jObject["m02"]?.Value<float>() ?? 0f),
				m03 = (jObject["m03"]?.Value<float>() ?? 0f),
				m10 = (jObject["m10"]?.Value<float>() ?? 0f),
				m11 = (jObject["m11"]?.Value<float>() ?? 0f),
				m12 = (jObject["m12"]?.Value<float>() ?? 0f),
				m13 = (jObject["m13"]?.Value<float>() ?? 0f),
				m20 = (jObject["m20"]?.Value<float>() ?? 0f),
				m21 = (jObject["m21"]?.Value<float>() ?? 0f),
				m22 = (jObject["m22"]?.Value<float>() ?? 0f),
				m23 = (jObject["m23"]?.Value<float>() ?? 0f),
				m30 = (jObject["m30"]?.Value<float>() ?? 0f),
				m31 = (jObject["m31"]?.Value<float>() ?? 0f),
				m32 = (jObject["m32"]?.Value<float>() ?? 0f),
				m33 = (jObject["m33"]?.Value<float>() ?? 0f)
			};
		}
	}
}
