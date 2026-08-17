using System;
using FishNet.Managing;

namespace FishNet.Serializing
{
	public sealed class PooledReader : Reader, IDisposable
	{
		internal PooledReader(byte[] bytes, NetworkManager networkManager, DataSource source = DataSource.Unset)
			: base(bytes, networkManager, null, source)
		{
		}

		internal PooledReader(ArraySegment<byte> segment, NetworkManager networkManager, DataSource source = DataSource.Unset)
			: base(segment, networkManager, null, source)
		{
		}

		public void Store()
		{
			ReaderPool.Store(this);
		}

		[Obsolete("Use Store().")]
		public void Dispose()
		{
			Store();
		}
	}
}
