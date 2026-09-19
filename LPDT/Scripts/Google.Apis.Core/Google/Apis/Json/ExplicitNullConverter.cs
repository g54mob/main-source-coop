using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Google.Apis.Json
{
	public class ExplicitNullConverter : JsonConverter
	{
		public override bool CanRead => false;

		public override bool CanConvert(Type objectType)
		{
			return objectType.GetTypeInfo().GetCustomAttributes(typeof(JsonExplicitNullAttribute), inherit: false).Any();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false.");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteNull();
		}
	}
}
