using System;
using System.Collections.Generic;

namespace VContainer.Unity
{
	internal sealed class LateTickableLoopItem : IPlayerLoopItem, IDisposable
	{
		private readonly IReadOnlyList<ILateTickable> entries;

		private readonly EntryPointExceptionHandler exceptionHandler;

		private bool disposed;

		public LateTickableLoopItem(IReadOnlyList<ILateTickable> entries, EntryPointExceptionHandler exceptionHandler)
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
					entries[i].LateTick();
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
