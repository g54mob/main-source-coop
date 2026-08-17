using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Managing;
using UnityEngine;

namespace FishNet.Serializing
{
	public static class WriterPool
	{
		private static readonly Stack<PooledWriter> _pool = new Stack<PooledWriter>();

		private static readonly Dictionary<int, Stack<PooledWriter>> _lengthPool = new Dictionary<int, Stack<PooledWriter>>();

		internal const int LENGTH_BRACKET = 1000;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Retrieve(NetworkManager).")]
		public static PooledWriter GetWriter(NetworkManager networkManager)
		{
			return Retrieve(networkManager);
		}

		public static PooledWriter Retrieve(NetworkManager networkManager)
		{
			PooledWriter obj = ((_pool.Count > 0) ? _pool.Pop() : new PooledWriter());
			obj.Reset(networkManager);
			return obj;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Retrieve().")]
		public static PooledWriter GetWriter()
		{
			return Retrieve();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PooledWriter Retrieve()
		{
			return Retrieve(null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Retrieve(int).")]
		public static PooledWriter GetWriter(int length)
		{
			return Retrieve(length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PooledWriter Retrieve(int length)
		{
			return Retrieve(null, length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Retrieve(NetworkManager, int).")]
		public static PooledWriter GetWriter(NetworkManager networkManager, int length)
		{
			return Retrieve(networkManager, length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PooledWriter Retrieve(NetworkManager networkManager, int length)
		{
			int dictionaryIndex = GetDictionaryIndex(length);
			if (_lengthPool.TryGetValue(dictionaryIndex, out var value) && value.Count > 0)
			{
				PooledWriter pooledWriter = value.Pop();
				pooledWriter.Reset(networkManager);
				return pooledWriter;
			}
			PooledWriter pooledWriter2 = Retrieve(networkManager);
			int count = (dictionaryIndex + 1) * 1000;
			pooledWriter2.EnsureBufferCapacity(count);
			return pooledWriter2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use StoreLength(PooledWriter).")]
		public static void RecycleLength(PooledWriter writer)
		{
			StoreLength(writer);
		}

		public static void StoreLength(PooledWriter writer)
		{
			int dictionaryIndex = GetDictionaryIndex(writer);
			if (!_lengthPool.TryGetValue(dictionaryIndex, out var value))
			{
				value = new Stack<PooledWriter>();
				_lengthPool[dictionaryIndex] = value;
			}
			value.Push(writer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use Store(PooledWriter).")]
		public static void Recycle(PooledWriter writer)
		{
			Store(writer);
		}

		public static void Store(PooledWriter writer)
		{
			_pool.Push(writer);
		}

		private static int GetDictionaryIndex(int length)
		{
			int num = Mathf.FloorToInt(length / 1000);
			if (num > 0 && length % 1000 == 0)
			{
				num--;
			}
			return num;
		}

		private static int GetDictionaryIndex(PooledWriter writer)
		{
			int num = writer.Capacity;
			if (num < 1000)
			{
				num = 1000;
				writer.EnsureBufferCapacity(1000);
			}
			return Mathf.FloorToInt(num / 1000) - 1;
		}
	}
}
