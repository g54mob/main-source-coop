using System;
using System.Collections.Generic;

namespace VContainer.Unity
{
	internal sealed class PostStartableLoopItem : IPlayerLoopItem, IDisposable
	{
		private readonly IEnumerable<IPostStartable> entries;

		private readonly EntryPointExceptionHandler exceptionHandler;

		private bool disposed;

		public PostStartableLoopItem(IEnumerable<IPostStartable> entries, EntryPointExceptionHandler exceptionHandler)
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
			foreach (IPostStartable entry in entries)
			{
				try
				{
					entry.PostStart();
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
