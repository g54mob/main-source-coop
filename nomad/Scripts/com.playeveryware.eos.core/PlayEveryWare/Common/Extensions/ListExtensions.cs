using System;
using System.Collections.Generic;
using System.Threading;

namespace PlayEveryWare.Common.Extensions
{
	public static class ListExtensions
	{
		private static class ThreadSafeRandom
		{
			private static readonly ThreadLocal<Random> threadLocalRandom = new ThreadLocal<Random>(() => new Random());

			public static Random Instance => threadLocalRandom.Value;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				lock (ThreadSafeRandom.Instance)
				{
					int num = ThreadSafeRandom.Instance.Next(i, count);
					int index = i;
					int index2 = num;
					T val = list[num];
					T val2 = list[i];
					T val3 = (list[index] = val);
					val3 = (list[index2] = val2);
				}
			}
		}
	}
}
