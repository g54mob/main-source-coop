using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Auth.OAuth2
{
	public interface IOidcTokenProvider
	{
		Task<OidcToken> GetOidcTokenAsync(OidcTokenOptions options, CancellationToken cancellationToken = default(CancellationToken));
	}
}
