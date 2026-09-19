using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Http
{
	public interface IHttpExecuteInterceptor
	{
		Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken);
	}
}
