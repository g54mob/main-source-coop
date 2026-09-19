using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public sealed class OidcToken
	{
		private readonly TokenRefreshManager _refreshManager;

		internal TokenResponse TokenResponse => _refreshManager.Token;

		internal OidcToken(TokenRefreshManager refreshManager)
		{
			_refreshManager = refreshManager.ThrowIfNull("refreshManager");
		}

		public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return _refreshManager.GetAccessTokenForRequestAsync(cancellationToken);
		}
	}
}
