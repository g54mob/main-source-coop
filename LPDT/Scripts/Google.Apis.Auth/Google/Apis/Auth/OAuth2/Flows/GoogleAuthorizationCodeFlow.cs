using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Json;

namespace Google.Apis.Auth.OAuth2.Flows
{
	public class GoogleAuthorizationCodeFlow : AuthorizationCodeFlow, IHttpAuthorizationFlow, IAuthorizationCodeFlow, IDisposable
	{
		public new class Initializer : AuthorizationCodeFlow.Initializer
		{
			public string ProjectId { get; set; }

			public string RevokeTokenUrl { get; set; }

			public bool? IncludeGrantedScopes { get; set; }

			public string LoginHint { get; set; }

			public string Prompt { get; set; }

			public string Nonce { get; set; }

			public IEnumerable<KeyValuePair<string, string>> UserDefinedQueryParams { get; set; }

			public Initializer()
				: this("https://accounts.google.com/o/oauth2/v2/auth", "https://oauth2.googleapis.com/token", "https://oauth2.googleapis.com/revoke")
			{
			}

			internal Initializer(GoogleAuthorizationCodeFlow flow)
				: base(flow)
			{
				ProjectId = flow.ProjectId;
				RevokeTokenUrl = flow.revokeTokenUrl;
				IncludeGrantedScopes = flow.IncludeGrantedScopes;
				LoginHint = flow.LoginHint;
				Prompt = flow.Prompt;
				Nonce = flow.Nonce;
				UserDefinedQueryParams = flow.UserDefinedQueryParams;
			}

			protected Initializer(string authorizationServerUrl, string tokenServerUrl, string revokeTokenUrl)
				: base(authorizationServerUrl, tokenServerUrl)
			{
				RevokeTokenUrl = revokeTokenUrl;
			}
		}

		private readonly string revokeTokenUrl;

		public readonly bool? includeGrantedScopes;

		private readonly IEnumerable<KeyValuePair<string, string>> userDefinedQueryParams;

		public string ProjectId { get; private set; }

		public string RevokeTokenUrl => revokeTokenUrl;

		public bool? IncludeGrantedScopes => includeGrantedScopes;

		public string LoginHint { get; private set; }

		public string Prompt { get; private set; }

		public string Nonce { get; private set; }

		public IEnumerable<KeyValuePair<string, string>> UserDefinedQueryParams => userDefinedQueryParams;

		public GoogleAuthorizationCodeFlow(Initializer initializer)
			: base(initializer)
		{
			ProjectId = initializer.ProjectId;
			revokeTokenUrl = initializer.RevokeTokenUrl;
			includeGrantedScopes = initializer.IncludeGrantedScopes;
			LoginHint = initializer.LoginHint;
			Prompt = initializer.Prompt;
			Nonce = initializer.Nonce;
			userDefinedQueryParams = initializer.UserDefinedQueryParams;
		}

		IHttpAuthorizationFlow IHttpAuthorizationFlow.WithHttpClientFactory(IHttpClientFactory httpClientFactory)
		{
			return new GoogleAuthorizationCodeFlow(new Initializer(this)
			{
				HttpClientFactory = httpClientFactory
			});
		}

		public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
		{
			return new GoogleAuthorizationCodeRequestUrl(new Uri(base.AuthorizationServerUrl))
			{
				ClientId = base.ClientSecrets.ClientId,
				Scope = string.Join(" ", base.Scopes),
				RedirectUri = redirectUri,
				IncludeGrantedScopes = (IncludeGrantedScopes.HasValue ? IncludeGrantedScopes.Value.ToString().ToLower() : null),
				LoginHint = LoginHint,
				Prompt = Prompt,
				Nonce = Nonce,
				UserDefinedQueryParams = UserDefinedQueryParams
			};
		}

		public override async Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken)
		{
			GoogleRevokeTokenRequest googleRevokeTokenRequest = new GoogleRevokeTokenRequest(new Uri(RevokeTokenUrl))
			{
				Token = token
			};
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, googleRevokeTokenRequest.Build());
			HttpResponseMessage response = await base.HttpClient.SendAsync(request, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!response.IsSuccessStatusCode)
			{
				string input = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				throw new TokenResponseException(NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(input), response.StatusCode);
			}
			await DeleteTokenAsync(userId, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public override bool ShouldForceTokenRetrieval()
		{
			if (IncludeGrantedScopes.HasValue)
			{
				return IncludeGrantedScopes.Value;
			}
			return false;
		}
	}
}
