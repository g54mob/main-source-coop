using System;
using System.Diagnostics;

namespace Mirror.SimpleWeb
{
	public class BufferPool
	{
		internal readonly BufferBucket[] buckets;

		private readonly int bucketCount;

		private readonly int smallest;

		private readonly int largest;

		public BufferPool(int bucketCount, int smallest, int largest)
		{
			if (bucketCount < 2)
			{
				throw new ArgumentException("Count must be at least 2");
			}
			if (smallest < 1)
			{
				throw new ArgumentException("Smallest must be at least 1");
			}
			if (largest < smallest)
			{
				throw new ArgumentException("Largest must be greater than smallest");
			}
			this.bucketCount = bucketCount;
			this.smallest = smallest;
			this.largest = largest;
			double num = Math.Log(this.smallest);
			double num2 = (Math.Log(this.largest) - num) / (double)(bucketCount - 1);
			buckets = new BufferBucket[bucketCount];
			for (int i = 0; i < bucketCount; i++)
			{
				double a = (double)smallest * Math.Pow(Math.E, num2 * (double)i);
				buckets[i] = new BufferBucket((int)Math.Ceiling(a));
			}
		}

		[Conditional("UNITY_ASSERTIONS")]
		private void Validate()
		{
			if (buckets[0].arraySize != smallest)
			{
				Log.Error("[SWT-BufferPool]: BufferPool Failed to create bucket for smallest. bucket:{0} smallest:{1}", buckets[0].arraySize, smallest);
			}
			int arraySize = buckets[bucketCount - 1].arraySize;
			if (arraySize != largest && arraySize != largest + 1)
			{
				Log.Error("[SWT-BufferPool]: BufferPool Failed to create bucket for largest. bucket:{0} smallest:{1}", arraySize, largest);
			}
		}

		public ArrayBuffer Take(int size)
		{
			if (size > largest)
			{
				throw new ArgumentException($"Size ({size}) is greater than largest ({largest})");
			}
			for (int i = 0; i < bucketCount; i++)
			{
				if (size <= buckets[i].arraySize)
				{
					return buckets[i].Take();
				}
			}
			throw new ArgumentException($"Size ({size}) is greater than largest ({largest})");
		}
	}
}
