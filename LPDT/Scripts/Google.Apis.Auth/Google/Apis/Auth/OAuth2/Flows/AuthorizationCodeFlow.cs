using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Logging;
using Google.Apis.Testing;
using Google.Apis.Util;
using Google.Apis.Util.Store;

namespace Google.Apis.Auth.OAuth2.Flows
{
	public class AuthorizationCodeFlow : IHttpAuthorizationFlow, IAuthorizationCodeFlow, IDisposable
	{
		public class Initializer
		{
			public IAccessMethod AccessMethod { get; set; }

			public string TokenServerUrl { get; private set; }

			public string AuthorizationServerUrl { get; private set; }

			public ClientSecrets ClientSecrets { get; set; }

			public Stream ClientSecretsStream { get; set; }

			public IDataStore DataStore { get; set; }

			public IEnumerable<string> Scopes { get; set; }

			public IHttpClientFactory HttpClientFactory { get; set; }

			public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

			public IClock Clock { get; set; }

			public Initializer(string authorizationServerUrl, string tokenServerUrl)
			{
				AuthorizationServerUrl = authorizationServerUrl;
				TokenServerUrl = tokenServerUrl;
				Scopes = new List<string>();
				AccessMethod = new BearerToken.AuthorizationHeaderAccessMethod();
				DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
				Clock = SystemClock.Default;
			}

			internal Initializer(AuthorizationCodeFlow flow)
			{
				AccessMethod = flow.AccessMethod;
				TokenServerUrl = flow.TokenServerUrl;
				AuthorizationServerUrl = flow.AuthorizationServerUrl;
				ClientSecrets = flow.ClientSecrets;
				DataStore = flow.DataStore;
				Scopes = flow.Scopes;
				HttpClientFactory = flow.HttpClientFactory;
				DefaultExponentialBackOffPolicy = flow.DefaultExponentialBackOffPolicy;
				Clock = flow.Clock;
			}
		}

		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<AuthorizationCodeFlow>();

		private readonly IAccessMethod accessMethod;

		private readonly string tokenServerUrl;

		private readonly string authorizationServerUrl;

		private readonly ClientSecrets clientSecrets;

		private readonly IDataStore dataStore;

		private readonly IEnumerable<string> scopes;

		private readonly ConfigurableHttpClient httpClient;

		private readonly IClock clock;

		public string TokenServerUrl => tokenServerUrl;

		public string AuthorizationServerUrl => authorizationServerUrl;

		public ClientSecrets ClientSecrets => clientSecrets;

		public IDataStore DataStore => dataStore;

		public IEnumerable<string> Scopes => scopes;

		public ConfigurableHttpClient HttpClient => httpClient;

		internal IHttpClientFactory HttpClientFactory { get; }

		private ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; }

		public IAccessMethod AccessMethod => accessMethod;

		public IClock Clock => clock;

		public AuthorizationCodeFlow(Initializer initializer)
		{
			clientSecrets = initializer.ClientSecrets;
			if (clientSecrets == null)
			{
				if (initializer.ClientSecretsStream == null)
				{
					throw new ArgumentException("You MUST set ClientSecret or ClientSecretStream on the initializer");
				}
				using (initializer.ClientSecretsStream)
				{
					clientSecrets = GoogleClientSecrets.FromStream(initializer.ClientSecretsStream).Secrets;
				}
			}
			else if (initializer.ClientSecretsStream != null)
			{
				throw new ArgumentException("You CAN'T set both ClientSecrets AND ClientSecretStream on the initializer");
			}
			accessMethod = initializer.AccessMethod.ThrowIfNull("Initializer.AccessMethod");
			clock = initializer.Clock.ThrowIfNull("Initializer.Clock");
			tokenServerUrl = initializer.TokenServerUrl.ThrowIfNullOrEmpty("Initializer.TokenServerUrl");
			authorizationServerUrl = initializer.AuthorizationServerUrl.ThrowIfNullOrEmpty("Initializer.AuthorizationServerUrl");
			dataStore = initializer.DataStore;
			if (dataStore == null)
			{
				Logger.Warning("Datastore is null, as a result the user's credential will not be stored");
			}
			scopes = initializer.Scopes;
			DefaultExponentialBackOffPolicy = initializer.DefaultExponentialBackOffPolicy;
			HttpClientFactory = initializer.HttpClientFactory ?? new HttpClientFactory();
			CreateHttpClientArgs createHttpClientArgs = new CreateHttpClientArgs();
			if (DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
			{
				createHttpClientArgs.Initializers.Add(new ExponentialBackOffInitializer(DefaultExponentialBackOffPolicy, () => new BackOffHandler(new ExponentialBackOff())));
			}
			httpClient = HttpClientFactory.CreateHttpClient(createHttpClientArgs);
		}

		IHttpAuthorizationFlow IHttpAuthorizationFlow.WithHttpClientFactory(IHttpClientFactory httpClientFactory)
		{
			return new AuthorizationCodeFlow(new Initializer(this)
			{
				HttpClientFactory = httpClientFactory
			});
		}

		public async Task<TokenResponse> LoadTokenAsync(string userId, CancellationToken taskCancellationToken)
		{
			taskCancellationToken.ThrowIfCancellationRequested();
			if (DataStore == null)
			{
				return null;
			}
			return await DataStore.GetAsync<TokenResponse>(userId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken)
		{
			taskCancellationToken.ThrowIfCancellationRequested();
			if (DataStore != null)
			{
				await DataStore.DeleteAsync<TokenResponse>(userId).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public virtual AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
		{
			return new AuthorizationCodeRequestUrl(new Uri(AuthorizationServerUrl))
			{
				ClientId = ClientSecrets.ClientId,
				Scope = ((Scopes == null) ? null : string.Join(" ", Scopes)),
				RedirectUri = redirectUri
			};
		}

		public async Task<TokenResponse> ExchangeCodeForTokenAsync(string userId, string code, string redirectUri, CancellationToken taskCancellationToken)
		{
			AuthorizationCodeTokenRequest request = new AuthorizationCodeTokenRequest
			{
				Scope = ((Scopes == null) ? null : string.Join(" ", Scopes)),
				RedirectUri = redirectUri,
				Code = code
			};
			TokenResponse token = await FetchTokenAsync(userId, request, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await StoreTokenAsync(userId, token, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return token;
		}

		public async Task<TokenResponse> RefreshTokenAsync(string userId, string refreshToken, CancellationToken taskCancellationToken)
		{
			RefreshTokenRequest request = new RefreshTokenRequest
			{
				RefreshToken = refreshToken
			};
			TokenResponse token = await FetchTokenAsync(userId, request, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (token.RefreshToken == null)
			{
				token.RefreshToken = refreshToken;
			}
			await StoreTokenAsync(userId, token, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return token;
		}

		public virtual Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken)
		{
			throw new NotImplementedException("The OAuth 2.0 protocol does not support token revocation.");
		}

		public virtual bool ShouldForceTokenRetrieval()
		{
			return false;
		}

		private async Task StoreTokenAsync(string userId, TokenResponse token, CancellationToken taskCancellationToken)
		{
			taskCancellationToken.ThrowIfCancellationRequested();
			if (DataStore != null)
			{
				await DataStore.StoreAsync(userId, token).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		[VisibleForTestOnly]
		public async Task<TokenResponse> FetchTokenAsync(string userId, TokenRequest request, CancellationToken taskCancellationToken)
		{
			request.ClientId = ClientSecrets.ClientId;
			request.ClientSecret = ClientSecrets.ClientSecret;
			try
			{
				return await request.ExecuteAsync(httpClient, TokenServerUrl, taskCancellationToken, Clock, Logger).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (TokenResponseException ex)
			{
				int valueOrDefault = (int)ex.StatusCode.GetValueOrDefault();
				if (valueOrDefault < 500 || valueOrDefault >= 600)
				{
					await DeleteTokenAsync(userId, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				throw;
			}
		}

		public void Dispose()
		{
			if (HttpClient != null)
			{
				HttpClient.Dispose();
			}
		}
	}
}
