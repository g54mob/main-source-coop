using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util;
using Google.Apis.Util.Store;

namespace Google.Apis.Auth.OAuth2.Flows
{
	public interface IAuthorizationCodeFlow : IDisposable
	{
		IAccessMethod AccessMethod { get; }

		IClock Clock { get; }

		IDataStore DataStore { get; }

		Task<TokenResponse> LoadTokenAsync(string userId, CancellationToken taskCancellationToken);

		Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken);

		AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri);

		Task<TokenResponse> ExchangeCodeForTokenAsync(string userId, string code, string redirectUri, CancellationToken taskCancellationToken);

		Task<TokenResponse> RefreshTokenAsync(string userId, string refreshToken, CancellationToken taskCancellationToken);

		Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken);

		bool ShouldForceTokenRetrieval();
	}
}
