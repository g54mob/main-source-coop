using System;
using System.Collections.Generic;
using System.Threading;

namespace Photon.Voice
{
	public class ArraySegmentPool<T> : ObjectFactory<ArraySegment<T>, int>, IDisposable
	{
		private const int SLOT_0_SIZE_LOG2 = 6;

		private const int MAX_SIZE_LOG2 = 16;

		private ArrayPool<T>[] pools = new ArrayPool<T>[11];

		private int capacity;

		private string name;

		private int defaultInfo;

		private static uint nlz(uint x)
		{
			uint num = 32u;
			uint num2 = x >> 16;
			if (num2 != 0)
			{
				num -= 16;
				x = num2;
			}
			num2 = x >> 8;
			if (num2 != 0)
			{
				num -= 8;
				x = num2;
			}
			num2 = x >> 4;
			if (num2 != 0)
			{
				num -= 4;
				x = num2;
			}
			num2 = x >> 2;
			if (num2 != 0)
			{
				num -= 2;
				x = num2;
			}
			if (x >> 1 != 0)
			{
				return num - 2;
			}
			return num - x;
		}

		private static int slot(uint x)
		{
			x--;
			int num = (int)(32 - nlz(x));
			return Math.Max(0, num - 6);
		}

		public ArraySegmentPool(int capacity, string name, int defaultInfo)
		{
			this.capacity = capacity;
			this.name = name;
			this.defaultInfo = defaultInfo;
		}

		public ArraySegment<T> New()
		{
			return New(defaultInfo);
		}

		public ArraySegment<T> New(int info)
		{
			if (info == 0)
			{
				return new ArraySegment<T>(Array.Empty<T>());
			}
			int num = slot((uint)info);
			if (num < pools.Length)
			{
				ArrayPool<T> arrayPool;
				lock (pools)
				{
					arrayPool = pools[num];
					if (arrayPool == null)
					{
						arrayPool = new ArrayPool<T>(capacity, name + " [" + num + "]", 64 << num);
						pools[num] = arrayPool;
					}
				}
				return new ArraySegment<T>(arrayPool.New(), 0, info);
			}
			throw new ArgumentException("ArraySegmentPool New size is too large: " + info);
		}

		public bool Free(ArraySegment<T> obj, int info)
		{
			return Free(obj);
		}

		public bool Free(ArraySegment<T> obj)
		{
			if (obj.Count == 0)
			{
				return false;
			}
			int num = slot((uint)obj.Array.Length);
			if (num < pools.Length)
			{
				return pools[num]?.Free(obj.Array) ?? false;
			}
			return false;
		}

		public void Dispose()
		{
			ArrayPool<T>[] array = pools;
			for (int i = 0; i < array.Length; i++)
			{
				array[i]?.Dispose();
			}
		}

		public static void test()
		{
			ObjectFactory<ArraySegment<T>, int> bufferFactory = new ArraySegmentPool<T>(100, " =======", 12345);
			Queue<ArraySegment<T>> test = new Queue<ArraySegment<T>>();
			for (int i = 0; i < 5; i++)
			{
				new Thread((ThreadStart)delegate
				{
					int num = 0;
					while (true)
					{
						lock (test)
						{
							if (test.Count < 100)
							{
								int num2 = num++;
								ArraySegment<T> item = bufferFactory.New(num2);
								if (num2 > 0)
								{
									item.Array[item.Offset + num2 - 1] = default(T);
								}
								test.Enqueue(item);
							}
						}
					}
				}).Start();
				new Thread((ThreadStart)delegate
				{
					while (true)
					{
						lock (test)
						{
							if (test.Count > 10)
							{
								bufferFactory.Free(test.Dequeue());
							}
						}
					}
				}).Start();
			}
		}
	}
}
