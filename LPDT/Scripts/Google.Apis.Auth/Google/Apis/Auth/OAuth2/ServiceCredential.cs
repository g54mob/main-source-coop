using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public abstract class ServiceCredential : ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders, IHttpExecuteInterceptor, IHttpUnsuccessfulResponseHandler
	{
		public class Initializer
		{
			public string TokenServerUrl { get; private set; }

			public IClock Clock { get; set; }

			public IAccessMethod AccessMethod { get; set; }

			public IHttpClientFactory HttpClientFactory { get; set; }

			public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

			public string QuotaProject { get; set; }

			internal IList<IConfigurableHttpClientInitializer> HttpClientInitializers { get; }

			public Initializer(string tokenServerUrl)
			{
				TokenServerUrl = tokenServerUrl;
				AccessMethod = new BearerToken.AuthorizationHeaderAccessMethod();
				Clock = SystemClock.Default;
				DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
				HttpClientInitializers = new List<IConfigurableHttpClientInitializer>();
			}

			internal Initializer(ServiceCredential other)
			{
				TokenServerUrl = other.TokenServerUrl;
				Clock = other.Clock;
				AccessMethod = other.AccessMethod;
				HttpClientFactory = other.HttpClientFactory;
				DefaultExponentialBackOffPolicy = other.DefaultExponentialBackOffPolicy;
				QuotaProject = other.QuotaProject;
				HttpClientInitializers = new List<IConfigurableHttpClientInitializer>(other.HttpClientInitializers);
			}

			internal Initializer(Initializer other)
			{
				TokenServerUrl = other.TokenServerUrl;
				Clock = other.Clock;
				AccessMethod = other.AccessMethod;
				HttpClientFactory = other.HttpClientFactory;
				DefaultExponentialBackOffPolicy = other.DefaultExponentialBackOffPolicy;
				QuotaProject = other.QuotaProject;
				HttpClientInitializers = new List<IConfigurableHttpClientInitializer>(other.HttpClientInitializers);
			}
		}

		protected static readonly ILogger Logger = ApplicationContext.Logger.ForType<ServiceCredential>();

		private readonly TokenRefreshManager _refreshManager;

		public string TokenServerUrl { get; }

		public IClock Clock { get; }

		public IAccessMethod AccessMethod { get; }

		public ConfigurableHttpClient HttpClient { get; }

		internal IHttpClientFactory HttpClientFactory { get; }

		internal IEnumerable<IConfigurableHttpClientInitializer> HttpClientInitializers { get; }

		internal ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; }

		public TokenResponse Token
		{
			get
			{
				return _refreshManager.Token;
			}
			set
			{
				_refreshManager.Token = value;
			}
		}

		public string QuotaProject { get; }

		public ServiceCredential(Initializer initializer)
		{
			TokenServerUrl = initializer.TokenServerUrl;
			AccessMethod = initializer.AccessMethod.ThrowIfNull("initializer.AccessMethod");
			Clock = initializer.Clock.ThrowIfNull("initializer.Clock");
			CreateHttpClientArgs createHttpClientArgs = new CreateHttpClientArgs();
			DefaultExponentialBackOffPolicy = initializer.DefaultExponentialBackOffPolicy;
			if (DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
			{
				createHttpClientArgs.Initializers.Add(new ExponentialBackOffInitializer(DefaultExponentialBackOffPolicy, () => new BackOffHandler(new ExponentialBackOff())));
			}
			HttpClientInitializers = new List<IConfigurableHttpClientInitializer>(initializer.HttpClientInitializers).AsReadOnly();
			foreach (IConfigurableHttpClientInitializer httpClientInitializer in HttpClientInitializers)
			{
				createHttpClientArgs.Initializers.Add(httpClientInitializer);
			}
			HttpClientFactory = initializer.HttpClientFactory ?? new HttpClientFactory();
			HttpClient = HttpClientFactory.CreateHttpClient(createHttpClientArgs);
			_refreshManager = new TokenRefreshManager(RequestAccessTokenAsync, Clock, Logger);
			QuotaProject = initializer.QuotaProject;
		}

		public void Initialize(ConfigurableHttpClient httpClient)
		{
			httpClient.MessageHandler.Credential = this;
		}

		public async Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string authUri = new UriBuilder(request.RequestUri)
			{
				Query = string.Empty,
				Path = string.Empty
			}.Uri.ToString();
			AccessTokenWithHeaders accessTokenWithHeaders = await GetAccessTokenWithHeadersForRequestAsync(authUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			AccessMethod.Intercept(request, accessTokenWithHeaders.AccessToken);
			accessTokenWithHeaders.AddHeaders(request);
		}

		public async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
		{
			if (args.Response.StatusCode == HttpStatusCode.Unauthorized)
			{
				bool flag = false;
				if (Token != null)
				{
					flag = object.Equals(Token.AccessToken, AccessMethod.GetAccessToken(args.Request));
				}
				bool flag2 = !flag;
				if (!flag2)
				{
					flag2 = await RequestAccessTokenAsync(args.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				return flag2;
			}
			return false;
		}

		public virtual Task<string> GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _refreshManager.GetAccessTokenForRequestAsync(cancellationToken);
		}

		public async Task<AccessTokenWithHeaders> GetAccessTokenWithHeadersForRequestAsync(string authUri = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			string token = await GetAccessTokenForRequestAsync(authUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return new AccessTokenWithHeaders.Builder
			{
				QuotaProject = QuotaProject
			}.Build(token);
		}

		public abstract Task<bool> RequestAccessTokenAsync(CancellationToken taskCancellationToken);
	}
}
