using System;
using System.Collections.Generic;

namespace Photon.Voice
{
	public class ArrayPoolSet<T> : ObjectFactory<T[], int>, IDisposable
	{
		private Dictionary<int, ArrayPool<T>> pools;

		private int capacity;

		private string name;

		private int defaultInfo;

		private int setSize;

		public ArrayPoolSet(int capacity, string name, int defaultInfo, int setSize)
		{
			this.capacity = capacity;
			this.name = name;
			this.defaultInfo = defaultInfo;
			this.setSize = setSize;
			pools = new Dictionary<int, ArrayPool<T>>(setSize);
		}

		public T[] New()
		{
			return New(defaultInfo);
		}

		public bool Free(T[] obj)
		{
			return Free(obj, defaultInfo);
		}

		public T[] New(int info)
		{
			ArrayPool<T> value;
			lock (pools)
			{
				if (!pools.TryGetValue(info, out value) && pools.Count < setSize)
				{
					value = new ArrayPool<T>(capacity, name + " [" + info + "]", info);
					pools[info] = value;
				}
			}
			if (value == null)
			{
				return new T[info];
			}
			return value.New();
		}

		public bool Free(T[] obj, int info)
		{
			ArrayPool<T> value;
			lock (pools)
			{
				pools.TryGetValue(info, out value);
			}
			return value?.Free(obj, info) ?? false;
		}

		public void Dispose()
		{
			lock (pools)
			{
				foreach (KeyValuePair<int, ArrayPool<T>> pool in pools)
				{
					pool.Value.Dispose();
				}
			}
		}
	}
}
