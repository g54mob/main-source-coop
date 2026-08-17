using System;
using Newtonsoft.Json;

namespace PlayEveryWare.EpicOnlineServices
{
	public class GuidConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is Guid guid)
			{
				writer.WriteValue(guid.ToString("N").ToLowerInvariant());
			}
			else
			{
				writer.WriteNull();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			string text = reader.Value?.ToString();
			if (string.IsNullOrEmpty(text))
			{
				return (objectType == typeof(Guid?)) ? ((Guid?)null) : new Guid?(Guid.Empty);
			}
			if (!Guid.TryParseExact(text, "N", out var result) && !Guid.TryParse(text, out result))
			{
				result = Guid.Empty;
			}
			return result;
		}

		public override bool CanConvert(Type objectType)
		{
			if (!(objectType == typeof(Guid)))
			{
				return objectType == typeof(Guid?);
			}
			return true;
		}
	}
}
