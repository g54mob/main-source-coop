using System;

namespace VContainer.Internal
{
	internal sealed class CappedArrayPool<T>
	{
		internal const int InitialBucketSize = 4;

		public static readonly CappedArrayPool<T> Shared8Limit = new CappedArrayPool<T>(8);

		private readonly T[][][] buckets;

		private readonly object syncRoot = new object();

		private readonly int[] tails;

		internal CappedArrayPool(int maxLength)
		{
			buckets = new T[maxLength][][];
			tails = new int[maxLength];
			for (int i = 0; i < maxLength; i++)
			{
				int num = i + 1;
				buckets[i] = new T[4][];
				for (int j = 0; j < 4; j++)
				{
					buckets[i][j] = new T[num];
				}
				tails[i] = 0;
			}
		}

		public T[] Rent(int length)
		{
			if (length <= 0)
			{
				return Array.Empty<T>();
			}
			if (length > buckets.Length)
			{
				return new T[length];
			}
			int num = length - 1;
			lock (syncRoot)
			{
				T[][] array = buckets[num];
				int num2 = tails[num];
				if (num2 >= array.Length)
				{
					Array.Resize(ref array, array.Length * 2);
					buckets[num] = array;
				}
				if (array[num2] == null)
				{
					array[num2] = new T[length];
				}
				T[] result = array[num2];
				tails[num]++;
				return result;
			}
		}

		public void Return(T[] array)
		{
			if (array.Length == 0 || array.Length > buckets.Length)
			{
				return;
			}
			int num = array.Length - 1;
			lock (syncRoot)
			{
				Array.Clear(array, 0, array.Length);
				if (tails[num] > 0)
				{
					tails[num]--;
				}
			}
		}
	}
}
