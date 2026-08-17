using System;
using System.Collections.Generic;

namespace VContainer.Unity
{
	internal sealed class FixedTickableLoopItem : IPlayerLoopItem, IDisposable
	{
		private readonly IReadOnlyList<IFixedTickable> entries;

		private readonly EntryPointExceptionHandler exceptionHandler;

		private bool disposed;

		public FixedTickableLoopItem(IReadOnlyList<IFixedTickable> entries, EntryPointExceptionHandler exceptionHandler)
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
			for (int i = 0; i < entries.Count; i++)
			{
				try
				{
					entries[i].FixedTick();
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
			return !disposed;
		}

		public void Dispose()
		{
			disposed = true;
		}
	}
}
