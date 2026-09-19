using System.Net;
using System.Net.Http;

namespace Google.Apis.Http
{
	public class HttpClientFactory : IHttpClientFactory
	{
		public IWebProxy Proxy { get; }

		public static HttpClientFactory ForProxy(IWebProxy proxy)
		{
			return new HttpClientFactory(proxy);
		}

		public HttpClientFactory()
			: this(null)
		{
		}

		protected HttpClientFactory(IWebProxy proxy)
		{
			Proxy = proxy;
		}

		public ConfigurableHttpClient CreateHttpClient(CreateHttpClientArgs args)
		{
			ConfigurableHttpClient configurableHttpClient = new ConfigurableHttpClient(new ConfigurableMessageHandler(CreateHandler(args))
			{
				ApplicationName = args.ApplicationName,
				GoogleApiClientHeader = args.GoogleApiClientHeader
			});
			foreach (IConfigurableHttpClientInitializer initializer in args.Initializers)
			{
				initializer.Initialize(configurableHttpClient);
			}
			return configurableHttpClient;
		}

		protected virtual HttpMessageHandler CreateHandler(CreateHttpClientArgs args)
		{
			HttpClientHandler httpClientHandler = CreateAndConfigureClientHandler();
			if (!args.GZipEnabled)
			{
				return new StreamInterceptionHandler(httpClientHandler);
			}
			if (!httpClientHandler.SupportsAutomaticDecompression)
			{
				return new GzipDeflateHandler(new StreamInterceptionHandler(httpClientHandler));
			}
			httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			return new TwoWayDelegatingHandler(httpClientHandler, new GzipDeflateHandler(new StreamInterceptionHandler(CreateAndConfigureClientHandler())), (HttpRequestMessage request) => StreamInterceptionHandler.GetInterceptorProvider(request) != null);
		}

		private HttpClientHandler CreateAndConfigureClientHandler()
		{
			HttpClientHandler httpClientHandler = CreateClientHandler();
			if (httpClientHandler.SupportsRedirectConfiguration)
			{
				httpClientHandler.AllowAutoRedirect = false;
			}
			if (httpClientHandler.SupportsAutomaticDecompression)
			{
				httpClientHandler.AutomaticDecompression = DecompressionMethods.None;
			}
			return httpClientHandler;
		}

		protected virtual HttpClientHandler CreateClientHandler()
		{
			HttpClientHandler httpClientHandler = new HttpClientHandler();
			if (Proxy != null)
			{
				httpClientHandler.Proxy = Proxy;
			}
			return httpClientHandler;
		}
	}
}
