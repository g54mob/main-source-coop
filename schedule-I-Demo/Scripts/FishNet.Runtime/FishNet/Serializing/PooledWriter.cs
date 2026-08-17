using System;

namespace FishNet.Serializing
{
	public sealed class PooledWriter : Writer, IDisposable
	{
		public void Store()
		{
			WriterPool.Store(this);
		}

		public void StoreLength()
		{
			WriterPool.StoreLength(this);
		}

		[Obsolete("Use Store().")]
		public void Dispose()
		{
			Store();
		}

		[Obsolete("Use StoreLength().")]
		public void DisposeLength()
		{
			StoreLength();
		}
	}
}
