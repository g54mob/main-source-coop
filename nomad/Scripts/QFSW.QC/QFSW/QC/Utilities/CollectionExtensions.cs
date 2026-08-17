using System;
using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC.Utilities
{
	public static class CollectionExtensions
	{
		public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> source)
		{
			Dictionary<TValue, TKey> dictionary = new Dictionary<TValue, TKey>();
			foreach (KeyValuePair<TKey, TValue> item in source)
			{
				if (!dictionary.ContainsKey(item.Value))
				{
					dictionary.Add(item.Value, item.Key);
				}
			}
			return dictionary;
		}

		public static T[] SubArray<T>(this T[] data, int index, int length)
		{
			T[] array = new T[length];
			Array.Copy(data, index, array, 0, length);
			return array;
		}

		public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source)
		{
			using IEnumerator<T> enumurator = source.GetEnumerator();
			if (enumurator.MoveNext())
			{
				T current = enumurator.Current;
				while (enumurator.MoveNext())
				{
					yield return current;
					current = enumurator.Current;
				}
			}
		}

		public static IEnumerable<T> Reversed<T>(this IReadOnlyList<T> source)
		{
			for (int i = source.Count - 1; i >= 0; i--)
			{
				yield return source[i];
			}
		}

		public static IEnumerable<TValue> DistinctBy<TValue, TDistinct>(this IEnumerable<TValue> source, Func<TValue, TDistinct> predicate)
		{
			HashSet<TDistinct> set = new HashSet<TDistinct>();
			foreach (TValue item in source)
			{
				if (set.Add(predicate(item)))
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<T> Yield<T>(this T item)
		{
			yield return item;
		}

		public static T LastOr<T>(this IEnumerable<T> source, T value)
		{
			try
			{
				return source.Last();
			}
			catch (InvalidOperationException)
			{
				return value;
			}
		}

		public unsafe static void InsertionSortBy<T>(this IList<T> collection, Func<T, int> keySelector)
		{
			if (collection.Count <= 512)
			{
				int* keyBuffer = stackalloc int[collection.Count];
				collection.InsertionSortBy(keySelector, keyBuffer);
			}
			else
			{
				fixed (int* keyBuffer2 = new int[collection.Count])
				{
					collection.InsertionSortBy(keySelector, keyBuffer2);
				}
			}
		}

		private unsafe static void InsertionSortBy<T>(this IList<T> collection, Func<T, int> keySelector, int* keyBuffer)
		{
			int count = collection.Count;
			for (int i = 0; i < count; i++)
			{
				keyBuffer[i] = keySelector(collection[i]);
			}
			for (int j = 1; j < count; j++)
			{
				T value = collection[j];
				int num = keyBuffer[j];
				int num2 = j - 1;
				while (num2 >= 0 && keyBuffer[num2] > num)
				{
					collection[num2 + 1] = collection[num2];
					keyBuffer[num2 + 1] = keyBuffer[num2];
					num2--;
				}
				collection[num2 + 1] = value;
				keyBuffer[num2 + 1] = num;
			}
		}
	}
}
