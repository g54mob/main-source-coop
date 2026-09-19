using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Auth.OAuth2
{
	public interface IBlobSigner
	{
		Task<string> SignBlobAsync(byte[] blob, CancellationToken cancellationToken = default(CancellationToken));
	}
}
