using System;
using MCPForUnity.Runtime.Helpers;
using Newtonsoft.Json;
using UnityEngine;

namespace MCPForUnity.Runtime.Serialization
{
	internal class UnityEngineObjectConverter : JsonConverter<UnityEngine.Object>
	{
		public override bool CanRead => true;

		public override bool CanWrite => true;

		public override void WriteJson(JsonWriter writer, UnityEngine.Object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteStartObject();
			writer.WritePropertyName("name");
			writer.WriteValue(value.name);
			WriteSerializedObjectId(writer, value);
			writer.WritePropertyName("warning");
			writer.WriteValue("UnityEngineObjectConverter running in non-Editor mode, asset path unavailable.");
			writer.WriteEndObject();
		}

		public override UnityEngine.Object ReadJson(JsonReader reader, Type objectType, UnityEngine.Object existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			Debug.LogWarning("UnityEngineObjectConverter cannot deserialize complex objects in non-Editor mode.");
			reader.Skip();
			return existingValue;
		}

		private static bool IsValidGuid(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string text = str.Replace("-", "");
			if (text.Length != 32)
			{
				return false;
			}
			string text2 = text;
			foreach (char c in text2)
			{
				if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
				{
					return false;
				}
			}
			return true;
		}

		private static void WriteSerializedObjectId(JsonWriter writer, UnityEngine.Object value)
		{
			writer.WritePropertyName("instanceID");
			writer.WriteValue(value.GetInstanceIDCompat());
		}
	}
}
