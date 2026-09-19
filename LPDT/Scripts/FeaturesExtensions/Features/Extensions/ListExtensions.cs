using System;
using System.Collections.Generic;

namespace Features.Extensions
{
	public static class ListExtensions
	{
		private static readonly Random _random = new Random();

		public static T SelectRandom<T>(this T[] list)
		{
			if (list == null || list.Length == 0)
			{
				return default(T);
			}
			int num = _random.Next(list.Length);
			return list[num];
		}

		public static T SelectRandom<T>(this IList<T> list)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			int index = _random.Next(list.Count);
			return list[index];
		}

		public static T SelectRandom<T>(this IReadOnlyList<T> list)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			int index = _random.Next(list.Count);
			return list[index];
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			if (list != null && list.Count > 1)
			{
				for (int num = list.Count - 1; num > 0; num--)
				{
					int num2 = _random.Next(num + 1);
					int index = num;
					int index2 = num2;
					T val = list[num2];
					T val2 = list[num];
					T val3 = (list[index] = val);
					val3 = (list[index2] = val2);
				}
			}
		}
	}
}
