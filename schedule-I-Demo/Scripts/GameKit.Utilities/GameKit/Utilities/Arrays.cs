using System;
using System.Collections.Generic;

namespace GameKit.Utilities
{
	public static class Arrays
	{
		private static Random _random = new Random();

		public static bool AddUnique<T>(this List<T> list, T value)
		{
			bool num = list.Contains(value);
			if (!num)
			{
				list.Add(value);
			}
			return !num;
		}

		public static bool FastReferenceRemove<T>(this List<T> list, object value)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if ((object)list[i] == value)
				{
					list.FastIndexRemove(i);
					return true;
				}
			}
			return false;
		}

		public static void FastIndexRemove<T>(this List<T> list, int index)
		{
			list[index] = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
		}

		public static void Shuffle<T>(this T[] array)
		{
			int num = array.Length;
			for (int i = 0; i < num - 1; i++)
			{
				int num2 = i + _random.Next(num - i);
				T val = array[num2];
				array[num2] = array[i];
				array[i] = val;
			}
		}

		public static void Shuffle<T>(this List<T> lst)
		{
			int count = lst.Count;
			for (int i = 0; i < count - 1; i++)
			{
				int index = i + _random.Next(count - i);
				T value = lst[index];
				lst[index] = lst[i];
				lst[i] = value;
			}
		}
	}
}
