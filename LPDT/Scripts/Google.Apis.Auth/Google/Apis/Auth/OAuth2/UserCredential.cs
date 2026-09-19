using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Logging;

namespace Google.Apis.Auth.OAuth2
{
	public class UserCredential : IGoogleCredential, ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders, IHttpExecuteInterceptor, IHttpUnsuccessfulResponseHandler
	{
		protected static readonly ILogger Logger = ApplicationContext.Logger.ForType<UserCredential>();

		private readonly TokenRefreshManager _refreshManager;

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

		public IAuthorizationCodeFlow Flow { get; }

		public string UserId { get; }

		public string QuotaProject { get; }

		bool IGoogleCredential.HasExplicitScopes => false;

		bool IGoogleCredential.SupportsExplicitScopes => false;

		public UserCredential(IAuthorizationCodeFlow flow, string userId, TokenResponse token)
			: this(flow, userId, token, null)
		{
		}

		public UserCredential(IAuthorizationCodeFlow flow, string userId, TokenResponse token, string quotaProjectId)
		{
			Flow = flow;
			UserId = userId;
			_refreshManager = new TokenRefreshManager(RefreshTokenAsync, flow.Clock, Logger);
			Token = token;
			QuotaProject = quotaProjectId;
		}

		IGoogleCredential IGoogleCredential.WithQuotaProject(string quotaProject)
		{
			return new UserCredential(Flow, UserId, Token, quotaProject);
		}

		IGoogleCredential IGoogleCredential.MaybeWithScopes(IEnumerable<string> scopes)
		{
			return this;
		}

		IGoogleCredential IGoogleCredential.WithUserForDomainWideDelegation(string user)
		{
			throw new InvalidOperationException("UserCredential does not support Domain-Wide Delegation");
		}

		IGoogleCredential IGoogleCredential.WithHttpClientFactory(IHttpClientFactory httpClientFactory)
		{
			if (!(Flow is IHttpAuthorizationFlow httpAuthorizationFlow))
			{
				throw new InvalidOperationException(Flow.GetType().FullName + " does not support an HTTP client factory to be set");
			}
			return new UserCredential(httpAuthorizationFlow.WithHttpClientFactory(httpClientFactory), UserId, Token, QuotaProject);
		}

		public async Task InterceptAsync(HttpRequestMessage request, CancellationToken taskCancellationToken)
		{
			AccessTokenWithHeaders accessTokenWithHeaders = await GetAccessTokenWithHeadersForRequestAsync(request.RequestUri.AbsoluteUri, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Flow.AccessMethod.Intercept(request, accessTokenWithHeaders.AccessToken);
			accessTokenWithHeaders.AddHeaders(request);
		}

		public async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
		{
			if (args.Response.StatusCode == HttpStatusCode.Unauthorized)
			{
				bool flag = !object.Equals(Token.AccessToken, Flow.AccessMethod.GetAccessToken(args.Request));
				if (!flag)
				{
					flag = await RefreshTokenAsync(args.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				return flag;
			}
			return false;
		}

		public void Initialize(ConfigurableHttpClient httpClient)
		{
			httpClient.MessageHandler.Credential = this;
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

		public async Task<bool> RefreshTokenAsync(CancellationToken taskCancellationToken)
		{
			if (Token.RefreshToken == null)
			{
				Logger.Warning("Refresh token is null, can't refresh the token!");
				return false;
			}
			TokenResponse tokenResponse = await Flow.RefreshTokenAsync(UserId, Token.RefreshToken, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Logger.Info("Access token was refreshed successfully");
			if (tokenResponse.RefreshToken == null)
			{
				tokenResponse.RefreshToken = Token.RefreshToken;
			}
			Token = tokenResponse;
			return true;
		}

		public async Task<bool> RevokeTokenAsync(CancellationToken taskCancellationToken)
		{
			if (Token == null)
			{
				Logger.Warning("Token is already null, no need to revoke it.");
				return false;
			}
			await Flow.RevokeTokenAsync(UserId, Token.AccessToken, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Logger.Info("Access token was revoked successfully");
			return true;
		}
	}
}
