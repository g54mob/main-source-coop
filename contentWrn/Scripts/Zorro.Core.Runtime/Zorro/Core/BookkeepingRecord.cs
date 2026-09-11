using System;

namespace Zorro.Core
{
	public abstract class BookkeepingRecord : IDisposable
	{
		public abstract void Dispose();

		public abstract void RemoveAtSwapBack(int index);
	}
}
