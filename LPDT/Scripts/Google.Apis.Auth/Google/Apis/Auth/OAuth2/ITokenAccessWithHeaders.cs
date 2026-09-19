using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Auth.OAuth2
{
	public interface ITokenAccessWithHeaders : ITokenAccess
	{
		Task<AccessTokenWithHeaders> GetAccessTokenWithHeadersForRequestAsync(string authUri = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
