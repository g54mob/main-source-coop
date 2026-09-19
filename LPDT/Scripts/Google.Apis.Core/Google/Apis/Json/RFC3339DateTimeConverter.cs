using System;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Json
{
	public class RFC3339DateTimeConverter : JsonConverter
	{
		public override bool CanRead => false;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false.");
		}

		public override bool CanConvert(Type objectType)
		{
			if (!(objectType == typeof(DateTime)))
			{
				return objectType == typeof(DateTime?);
			}
			return true;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value != null)
			{
				DateTime date = (DateTime)value;
				serializer.Serialize(writer, Utilities.ConvertToRFC3339(date));
			}
		}
	}
}
