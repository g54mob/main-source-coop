using System;
using System.Collections.Generic;

namespace VContainer.Unity
{
	internal sealed class StartableLoopItem : IPlayerLoopItem, IDisposable
	{
		private readonly IEnumerable<IStartable> entries;

		private readonly EntryPointExceptionHandler exceptionHandler;

		private bool disposed;

		public StartableLoopItem(IEnumerable<IStartable> entries, EntryPointExceptionHandler exceptionHandler)
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
			foreach (IStartable entry in entries)
			{
				try
				{
					entry.Start();
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
			disposed = true;
		}
	}
}
