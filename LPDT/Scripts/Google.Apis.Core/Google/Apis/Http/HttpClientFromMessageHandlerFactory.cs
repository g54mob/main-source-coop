using System;
using System.Net.Http;
using Google.Apis.Util;

namespace Google.Apis.Http
{
	public sealed class HttpClientFromMessageHandlerFactory : IHttpClientFactory
	{
		public sealed class HttpMessageHandlerOptions
		{
			internal static readonly HttpMessageHandlerOptions AllowDecompressionOptions = new HttpMessageHandlerOptions(mayPerformDecompression: true, mayHandleRedirects: false);

			internal static readonly HttpMessageHandlerOptions DisallowDecompressionOptions = new HttpMessageHandlerOptions(mayPerformDecompression: false, mayHandleRedirects: false);

			public bool MayPerformDecompression { get; }

			public bool MayHandleRedirects { get; }

			private HttpMessageHandlerOptions(bool mayPerformDecompression, bool mayHandleRedirects)
			{
				MayPerformDecompression = mayPerformDecompression;
				MayHandleRedirects = mayHandleRedirects;
			}

			internal ConfiguredHttpMessageHandler CheckItMatches(ConfiguredHttpMessageHandler handler)
			{
				if (handler == null)
				{
					throw new InvalidOperationException("The given HTTP message handler factory returned null.");
				}
				if (handler.MessageHandler == null)
				{
					throw new InvalidOperationException("The given HTTP message handler factory returned a null MessageHandler.");
				}
				if (!MayPerformDecompression && handler.PerformsAutomaticDecompression)
				{
					throw new InvalidOperationException("A handler that does not perform decompression was requested, but the HTTP message handler factory returned one that does.");
				}
				if (!MayHandleRedirects && handler.HandlesRedirects)
				{
					throw new InvalidOperationException("A handler that does not handles redirects was requested, but the HTTP message handler factory returned one that does.");
				}
				return handler;
			}
		}

		public sealed class ConfiguredHttpMessageHandler
		{
			public HttpMessageHandler MessageHandler { get; }

			public bool PerformsAutomaticDecompression { get; }

			public bool HandlesRedirects { get; }

			public ConfiguredHttpMessageHandler(HttpMessageHandler messageHandler, bool performsAutomaticDecompression, bool handlesRedirect)
			{
				MessageHandler = messageHandler;
				PerformsAutomaticDecompression = performsAutomaticDecompression;
				HandlesRedirects = handlesRedirect;
			}
		}

		private readonly Func<HttpMessageHandlerOptions, ConfiguredHttpMessageHandler> _httpMessageHandlerFactory;

		public HttpClientFromMessageHandlerFactory(Func<HttpMessageHandlerOptions, ConfiguredHttpMessageHandler> httpMessageHandlerFactory)
		{
			_httpMessageHandlerFactory = httpMessageHandlerFactory.ThrowIfNull("httpMessageHandlerFactory");
		}

		public ConfigurableHttpClient CreateHttpClient(CreateHttpClientArgs args)
		{
			ConfigurableHttpClient configurableHttpClient = new ConfigurableHttpClient(new ConfigurableMessageHandler(CreateHandler(args))
			{
				ApplicationName = args.ApplicationName,
				GoogleApiClientHeader = args.GoogleApiClientHeader
			}, disposeHandler: false);
			foreach (IConfigurableHttpClientInitializer initializer in args.Initializers)
			{
				initializer.Initialize(configurableHttpClient);
			}
			return configurableHttpClient;
		}

		private HttpMessageHandler CreateHandler(CreateHttpClientArgs args)
		{
			HttpMessageHandlerOptions httpMessageHandlerOptions = (args.GZipEnabled ? HttpMessageHandlerOptions.AllowDecompressionOptions : HttpMessageHandlerOptions.DisallowDecompressionOptions);
			ConfiguredHttpMessageHandler configuredHttpMessageHandler = httpMessageHandlerOptions.CheckItMatches(_httpMessageHandlerFactory(httpMessageHandlerOptions));
			if (!httpMessageHandlerOptions.MayPerformDecompression)
			{
				return new StreamInterceptionHandler(configuredHttpMessageHandler.MessageHandler);
			}
			if (configuredHttpMessageHandler.PerformsAutomaticDecompression)
			{
				ConfiguredHttpMessageHandler configuredHttpMessageHandler2 = HttpMessageHandlerOptions.DisallowDecompressionOptions.CheckItMatches(_httpMessageHandlerFactory(HttpMessageHandlerOptions.DisallowDecompressionOptions));
				return new TwoWayDelegatingHandler(configuredHttpMessageHandler.MessageHandler, new GzipDeflateHandler(new StreamInterceptionHandler(configuredHttpMessageHandler2.MessageHandler)), (HttpRequestMessage request) => StreamInterceptionHandler.GetInterceptorProvider(request) != null);
			}
			return new GzipDeflateHandler(new StreamInterceptionHandler(configuredHttpMessageHandler.MessageHandler));
		}
	}
}
