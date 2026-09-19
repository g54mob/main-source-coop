using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Http;
using Google.Apis.Json;
using Google.Apis.Logging;
using Google.Apis.Requests;
using Google.Apis.Responses;
using Google.Apis.Testing;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Services
{
	public abstract class BaseClientService : IClientService, IDisposable
	{
		public class Initializer
		{
			private const string InvalidApplicationNameCharacters = "\"(),:;<=>?@[\\]{}";

			public IHttpClientFactory HttpClientFactory { get; set; }

			public IConfigurableHttpClientInitializer HttpClientInitializer { get; set; }

			public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

			public bool GZipEnabled { get; set; }

			public ISerializer Serializer { get; set; }

			public string ApiKey { get; set; }

			public string ApplicationName { get; set; }

			public uint MaxUrlLength { get; set; }

			public string BaseUri { get; set; }

			public VersionHeaderBuilder VersionHeaderBuilder { get; }

			public bool ValidateParameters { get; set; } = true;

			public Initializer()
			{
				GZipEnabled = true;
				Serializer = new NewtonsoftJsonSerializer();
				DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
				MaxUrlLength = 2048u;
				VersionHeaderBuilder = new VersionHeaderBuilder().AppendDotNetEnvironment();
			}

			internal void Validate()
			{
				if (ApplicationName != null && ApplicationName.Any((char c) => Enumerable.Contains("\"(),:;<=>?@[\\]{}", c)))
				{
					throw new ArgumentException("Invalid Application name", "ApplicationName");
				}
			}
		}

		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<BaseClientService>();

		[VisibleForTestOnly]
		public const uint DefaultMaxUrlLength = 2048u;

		internal bool ValidateParameters { get; }

		protected string BaseUriOverride { get; }

		public ConfigurableHttpClient HttpClient { get; private set; }

		public IConfigurableHttpClientInitializer HttpClientInitializer { get; private set; }

		public bool GZipEnabled { get; private set; }

		public string ApiKey { get; private set; }

		public string ApplicationName { get; private set; }

		public ISerializer Serializer { get; private set; }

		public abstract string Name { get; }

		public abstract string BaseUri { get; }

		public abstract string BasePath { get; }

		public virtual string BatchUri => null;

		public virtual string BatchPath => null;

		public abstract IList<string> Features { get; }

		protected BaseClientService(Initializer initializer)
		{
			initializer.Validate();
			string versionHeader = initializer.VersionHeaderBuilder.Clone().AppendAssemblyVersion("gdcl", GetType()).ToString();
			GZipEnabled = initializer.GZipEnabled;
			Serializer = initializer.Serializer;
			ApiKey = initializer.ApiKey;
			ApplicationName = initializer.ApplicationName;
			BaseUriOverride = initializer.BaseUri;
			ValidateParameters = initializer.ValidateParameters;
			if (ApplicationName == null)
			{
				Logger.Warning("Application name is not set. Please set Initializer.ApplicationName property");
			}
			HttpClientInitializer = initializer.HttpClientInitializer;
			HttpClient = CreateHttpClient(initializer, versionHeader);
		}

		private bool HasFeature(Features feature)
		{
			return Features.Contains(Utilities.GetEnumStringValue(feature));
		}

		private ConfigurableHttpClient CreateHttpClient(Initializer initializer, string versionHeader)
		{
			IHttpClientFactory obj = initializer.HttpClientFactory ?? new HttpClientFactory();
			CreateHttpClientArgs createHttpClientArgs = new CreateHttpClientArgs
			{
				GZipEnabled = GZipEnabled,
				ApplicationName = ApplicationName,
				GoogleApiClientHeader = versionHeader
			};
			if (HttpClientInitializer != null)
			{
				createHttpClientArgs.Initializers.Add(HttpClientInitializer);
			}
			if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
			{
				createHttpClientArgs.Initializers.Add(new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, CreateBackOffHandler));
			}
			ConfigurableHttpClient configurableHttpClient = obj.CreateHttpClient(createHttpClientArgs);
			if (initializer.MaxUrlLength != 0)
			{
				configurableHttpClient.MessageHandler.AddExecuteInterceptor(new MaxUrlLengthInterceptor(initializer.MaxUrlLength));
			}
			return configurableHttpClient;
		}

		protected virtual BackOffHandler CreateBackOffHandler()
		{
			return new BackOffHandler(new ExponentialBackOff());
		}

		public void SetRequestSerailizedContent(HttpRequestMessage request, object body)
		{
			request.SetRequestSerailizedContent(this, body, GZipEnabled);
		}

		public virtual string SerializeObject(object obj)
		{
			return Serializer.Serialize(obj);
		}

		public virtual async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
		{
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (object.Equals(typeof(T), typeof(string)))
			{
				return (T)(object)text;
			}
			if (HasFeature(Google.Apis.Discovery.Features.LegacyDataResponse))
			{
				StandardResponse<T> standardResponse;
				try
				{
					standardResponse = Serializer.Deserialize<StandardResponse<T>>(text);
				}
				catch (JsonReaderException inner)
				{
					throw new GoogleApiException(Name, "Failed to parse response from server as json [" + text + "]", inner);
				}
				if (standardResponse.Error != null)
				{
					throw new GoogleApiException(Name, "Server error - " + standardResponse.Error)
					{
						Error = standardResponse.Error
					};
				}
				if (standardResponse.Data == null)
				{
					throw new GoogleApiException(Name, "The response could not be deserialized.");
				}
				return standardResponse.Data;
			}
			T val;
			try
			{
				val = Serializer.Deserialize<T>(text);
			}
			catch (JsonReaderException inner2)
			{
				throw new GoogleApiException(Name, "Failed to parse response from server as json [" + text + "]", inner2);
			}
			string text2 = ((response.Headers.ETag != null) ? response.Headers.ETag.Tag : null);
			if (val is IDirectResponseSchema && text2 != null)
			{
				(val as IDirectResponseSchema).ETag = text2;
			}
			return val;
		}

		public virtual Task<RequestError> DeserializeError(HttpResponseMessage response)
		{
			return response.DeserializeErrorAsync(Name, Serializer);
		}

		public virtual void Dispose()
		{
			if (HttpClient != null)
			{
				HttpClient.Dispose();
			}
		}
	}
}
