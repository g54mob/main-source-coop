using System.Threading.Tasks;

namespace Google.Apis.Http
{
	public interface IHttpUnsuccessfulResponseHandler
	{
		Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args);
	}
}
