using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Auth.OAuth2
{
	public interface ITokenAccess
	{
		Task<string> GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
