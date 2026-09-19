using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Logging;

namespace Google.Apis.Auth.OAuth2
{
	public class AuthorizationCodeInstalledApp : IAuthorizationCodeInstalledApp
	{
		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<AuthorizationCodeInstalledApp>();

		private readonly IAuthorizationCodeFlow flow;

		private readonly ICodeReceiver codeReceiver;

		public IAuthorizationCodeFlow Flow => flow;

		public ICodeReceiver CodeReceiver => codeReceiver;

		public AuthorizationCodeInstalledApp(IAuthorizationCodeFlow flow, ICodeReceiver codeReceiver)
		{
			this.flow = flow;
			this.codeReceiver = codeReceiver;
		}

		public async Task<UserCredential> AuthorizeAsync(string userId, CancellationToken taskCancellationToken)
		{
			TokenResponse token = await Flow.LoadTokenAsync(userId, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (ShouldRequestAuthorizationCode(token))
			{
				string redirectUri = CodeReceiver.RedirectUri;
				AuthorizationCodeRequestUrl url = Flow.CreateAuthorizationCodeRequest(redirectUri);
				AuthorizationCodeResponseUrl authorizationCodeResponseUrl = await CodeReceiver.ReceiveCodeAsync(url, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrEmpty(authorizationCodeResponseUrl.Code))
				{
					TokenErrorResponse tokenErrorResponse = new TokenErrorResponse(authorizationCodeResponseUrl);
					Logger.Info("Received an error. The response is: {0}", tokenErrorResponse);
					throw new TokenResponseException(tokenErrorResponse);
				}
				Logger.Debug("Received \"{0}\" code", authorizationCodeResponseUrl.Code);
				token = await Flow.ExchangeCodeForTokenAsync(userId, authorizationCodeResponseUrl.Code, redirectUri, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return new UserCredential(flow, userId, token);
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
