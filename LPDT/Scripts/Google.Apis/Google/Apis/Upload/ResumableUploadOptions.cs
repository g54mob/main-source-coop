using System;
using System.Net.Http;
using Google.Apis.Http;

namespace Google.Apis.Upload
{
	public sealed class ResumableUploadOptions
	{
		public HttpClient HttpClient { get; set; }

		public Action<HttpRequestMessage> ModifySessionInitiationRequest { get; set; }

		public ISerializer Serializer { get; set; }

		public string ServiceName { get; set; }

		internal ConfigurableHttpClient ConfigurableHttpClient => HttpClient as ConfigurableHttpClient;
	}
}
