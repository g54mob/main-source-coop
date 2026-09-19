using System;
using System.Net.Http;
using System.Threading;

namespace Google.Apis.Http
{
	public class HandleExceptionArgs
	{
		public HttpRequestMessage Request { get; set; }

		public Exception Exception { get; set; }

		public int TotalTries { get; set; }

		public int CurrentFailedTry { get; set; }

		public bool SupportsRetry => TotalTries - CurrentFailedTry > 0;

		public CancellationToken CancellationToken { get; set; }
	}
}
