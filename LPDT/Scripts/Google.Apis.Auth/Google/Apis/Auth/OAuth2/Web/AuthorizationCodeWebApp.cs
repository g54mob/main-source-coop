using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;

namespace Google.Apis.Auth.OAuth2.Web
{
	public class AuthorizationCodeWebApp
	{
		public class AuthResult
		{
			public UserCredential Credential { get; set; }

			public string RedirectUri { get; set; }
		}

		public const string StateKey = "oauth_";

		public const int StateRandomLength = 8;

		private readonly IAuthorizationCodeFlow flow;

		private readonly string redirectUri;

		private readonly string state;

		public IAuthorizationCodeFlow Flow => flow;

		public string RedirectUri => redirectUri;

		public string State => state;

		public AuthorizationCodeWebApp(IAuthorizationCodeFlow flow, string redirectUri, string state)
		{
			this.flow = flow;
			this.redirectUri = redirectUri;
			this.state = state;
		}

		public async Task<AuthResult> AuthorizeAsync(string userId, CancellationToken taskCancellationToken)
		{
			TokenResponse token = await Flow.LoadTokenAsync(userId, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (ShouldRequestAuthorizationCode(token))
			{
				AuthorizationCodeRequestUrl codeRequest = Flow.CreateAuthorizationCodeRequest(redirectUri);
				string oauthState = state;
				if (Flow.DataStore != null)
				{
					string s = new string('9', 8);
					string text = new Random().Next(int.Parse(s)).ToString("D" + 8);
					oauthState += text;
					await Flow.DataStore.StoreAsync("oauth_" + userId, oauthState).ConfigureAwait(continueOnCapturedContext: false);
				}
				codeRequest.State = oauthState;
				return new AuthResult
				{
					RedirectUri = codeRequest.Build().AbsoluteUri
				};
			}
			return new AuthResult
			{
				Credential = new UserCredential(flow, userId, token)
			};
		}

		public bool ShouldRequestAuthorizationCode(TokenResponse token)
		{
			if (!Flow.ShouldForceTokenRetrieval() && token != null)
			{
				if (token.RefreshToken == null)
				{
					return token.IsExpired(flow.Clock);
				}
				return false;
			}
			return true;
		}
	}
}
