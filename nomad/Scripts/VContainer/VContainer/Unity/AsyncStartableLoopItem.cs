using System;
using System.Collections.Generic;
using System.Threading;

namespace VContainer.Unity
{
	internal sealed class AsyncStartableLoopItem : IPlayerLoopItem, IDisposable
	{
		private readonly IEnumerable<IAsyncStartable> entries;

		private readonly EntryPointExceptionHandler exceptionHandler;

		private readonly CancellationTokenSource cts = new CancellationTokenSource();

		private bool disposed;

		public AsyncStartableLoopItem(IEnumerable<IAsyncStartable> entries, EntryPointExceptionHandler exceptionHandler)
		{
			this.entries = entries;
			this.exceptionHandler = exceptionHandler;
		}

		public bool MoveNext()
		{
			if (disposed)
			{
				return false;
			}
			foreach (IAsyncStartable entry in entries)
			{
				try
				{
					entry.StartAsync(cts.Token).Forget(exceptionHandler);
				}
				catch (Exception ex)
				{
					if (exceptionHandler == null)
					{
						throw;
					}
					exceptionHandler.Publish(ex);
				}
			}
			return false;
		}

		public void Dispose()
		{
			lock (entries)
			{
				if (disposed)
				{
					return;
				}
				disposed = true;
			}
			cts.Cancel();
			cts.Dispose();
		}
	}
}
