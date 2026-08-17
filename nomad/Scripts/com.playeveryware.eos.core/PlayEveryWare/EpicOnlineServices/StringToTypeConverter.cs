using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlayEveryWare.EpicOnlineServices
{
	public class StringToTypeConverter<T> : JsonConverter
	{
		private readonly Type _targetType = typeof(T);

		private readonly Type _underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

		public override bool CanConvert(Type objectType)
		{
			return _targetType.IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);
			if (jToken.Type == JTokenType.Null && Nullable.GetUnderlyingType(objectType) != null)
			{
				return null;
			}
			try
			{
				return ConvertToken(jToken, _underlyingType);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException($"Error converting token '{jToken}' to type '{_targetType}'.", innerException);
			}
		}

		private static object ConvertToken(JToken token, Type targetType)
		{
			string text = token.Value<string>();
			if (string.IsNullOrEmpty(text))
			{
				text = default(T).ToString();
			}
			switch (token.Type)
			{
			case JTokenType.String:
				if (targetType.IsEnum)
				{
					return Enum.Parse(targetType, text);
				}
				if (targetType == typeof(Guid))
				{
					return Guid.Parse(text);
				}
				return TypeDescriptor.GetConverter(targetType).ConvertFromInvariantString(text);
			case JTokenType.Integer:
			case JTokenType.Float:
				return token.ToObject(targetType);
			case JTokenType.Guid:
				if (targetType == typeof(Guid))
				{
					return token.ToObject<Guid>();
				}
				break;
			}
			throw new JsonSerializationException($"Unexpected token type '{token.Type}' when parsing to type '{targetType}'.");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value);
		}
	}
}
