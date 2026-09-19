using System;
using System.Threading;

namespace Photon.Realtime
{
	public class ConnectionServiceScope : IDisposable
	{
		private CancellationTokenSource cancellationTokenSource;

		public ConnectionServiceScope(RealtimeClient client, AsyncConfig config = null)
		{
			cancellationTokenSource = new CancellationTokenSource();
			client.CreateServiceTask(cancellationTokenSource.Token, null, null, config.Resolve());
		}

		public void Dispose()
		{
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();
			cancellationTokenSource = null;
		}
	}
}
