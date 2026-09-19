using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Http;
using Google.Apis.Requests;

namespace Google.Apis.Services
{
	public interface IClientService : IDisposable
	{
		ConfigurableHttpClient HttpClient { get; }

		IConfigurableHttpClientInitializer HttpClientInitializer { get; }

		string Name { get; }

		string BaseUri { get; }

		string BasePath { get; }

		IList<string> Features { get; }

		bool GZipEnabled { get; }

		string ApiKey { get; }

		string ApplicationName { get; }

		ISerializer Serializer { get; }

		void SetRequestSerailizedContent(HttpRequestMessage request, object body);

		string SerializeObject(object data);

		Task<T> DeserializeResponse<T>(HttpResponseMessage response);

		Task<RequestError> DeserializeError(HttpResponseMessage response);
	}
}
