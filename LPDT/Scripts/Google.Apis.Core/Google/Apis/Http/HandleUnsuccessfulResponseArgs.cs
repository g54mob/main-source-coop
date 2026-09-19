using System.Net.Http;
using System.Threading;

namespace Google.Apis.Http
{
	public class HandleUnsuccessfulResponseArgs
	{
		public HttpRequestMessage Request { get; set; }

		public HttpResponseMessage Response { get; set; }

		public int TotalTries { get; set; }

		public int CurrentFailedTry { get; set; }

		public bool SupportsRetry => TotalTries - CurrentFailedTry > 0;

		public CancellationToken CancellationToken { get; set; }
	}
}
