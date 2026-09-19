using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Google.Apis.Json
{
	public class NewtonsoftJsonContractResolver : DefaultContractResolver
	{
		private static readonly JsonConverter DateTimeConverter = new RFC3339DateTimeConverter();

		private static readonly JsonConverter ExplicitNullConverter = new ExplicitNullConverter();

		protected override JsonContract CreateContract(Type objectType)
		{
			JsonContract jsonContract = base.CreateContract(objectType);
			if (DateTimeConverter.CanConvert(objectType))
			{
				jsonContract.Converter = DateTimeConverter;
			}
			else if (ExplicitNullConverter.CanConvert(objectType))
			{
				jsonContract.Converter = ExplicitNullConverter;
			}
			return jsonContract;
		}
	}
}
