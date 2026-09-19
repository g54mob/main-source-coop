using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Json
{
	public class NewtonsoftJsonSerializer : IJsonSerializer, ISerializer
	{
		private readonly JsonSerializerSettings settings;

		private readonly JsonSerializer serializer;

		public static NewtonsoftJsonSerializer Instance { get; } = new NewtonsoftJsonSerializer();

		public string Format => "json";

		public NewtonsoftJsonSerializer()
			: this(CreateDefaultSettings())
		{
		}

		public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
		{
			settings.ThrowIfNull("settings");
			this.settings = settings;
			serializer = JsonSerializer.Create(settings);
		}

		public static JsonSerializerSettings CreateDefaultSettings()
		{
			return new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
				ContractResolver = new NewtonsoftJsonContractResolver()
			};
		}

		public void Serialize(object obj, Stream target)
		{
			using StreamWriter textWriter = new StreamWriter(target);
			if (obj == null)
			{
				obj = string.Empty;
			}
			serializer.Serialize(textWriter, obj);
		}

		public string Serialize(object obj)
		{
			using TextWriter textWriter = new StringWriter();
			if (obj == null)
			{
				obj = string.Empty;
			}
			serializer.Serialize(textWriter, obj);
			return textWriter.ToString();
		}

		public T Deserialize<T>(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return default(T);
			}
			return JsonConvert.DeserializeObject<T>(input, settings);
		}

		public object Deserialize(string input, Type type)
		{
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}
			return JsonConvert.DeserializeObject(input, type, settings);
		}

		public T Deserialize<T>(Stream input)
		{
			using StreamReader reader = new StreamReader(input);
			return (T)serializer.Deserialize(reader, typeof(T));
		}

		public async Task<T> DeserializeAsync<T>(Stream input, CancellationToken cancellationToken)
		{
			using StreamReader streamReader = new StreamReader(input);
			using JsonTextReader reader = new JsonTextReader(new StringReader(await streamReader.ReadToEndAsync().WithCancellationToken(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
			return (T)serializer.Deserialize(reader, typeof(T));
		}
	}
}
