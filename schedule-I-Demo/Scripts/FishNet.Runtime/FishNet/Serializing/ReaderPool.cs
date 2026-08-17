using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Managing;

namespace FishNet.Serializing
{
	public static class ReaderPool
	{
		private static readonly Stack<PooledReader> _pool = new Stack<PooledReader>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Retrieve(byte[], NetworkManager, DataSource)")]
		public static PooledReader GetReader(byte[] bytes, NetworkManager networkManager, Reader.DataSource source = Reader.DataSource.Unset)
		{
			return Retrieve(bytes, networkManager, source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PooledReader Retrieve(byte[] bytes, NetworkManager networkManager, Reader.DataSource source = Reader.DataSource.Unset)
		{
			return Retrieve(new ArraySegment<byte>(bytes), networkManager, source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Retrieve(ArraySegment, NetworkManager, DataSource)")]
		public static PooledReader GetReader(ArraySegment<byte> segment, NetworkManager networkManager, Reader.DataSource source = Reader.DataSource.Unset)
		{
			return Retrieve(segment, networkManager, source);
		}

		public static PooledReader Retrieve(ArraySegment<byte> segment, NetworkManager networkManager, Reader.DataSource source = Reader.DataSource.Unset)
		{
			PooledReader pooledReader;
			if (_pool.Count > 0)
			{
				pooledReader = _pool.Pop();
				pooledReader.Initialize(segment, networkManager, source);
			}
			else
			{
				pooledReader = new PooledReader(segment, networkManager, source);
			}
			return pooledReader;
		}

		[Obsolete("Use Store(PooledReader)")]
		public static void Recycle(PooledReader reader)
		{
			Store(reader);
		}

		public static void Store(PooledReader reader)
		{
			_pool.Push(reader);
		}
	}
}
