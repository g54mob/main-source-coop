using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Extensions
{
	[PublicAPI]
	public static class MyCollections
	{
		public static T[] InsertAt<T>(this T[] array, int index, T item)
		{
			if (index < 0)
			{
				Debug.LogError("Index is less than zero. Array is not modified");
				return array;
			}
			if (index > array.Length)
			{
				Debug.LogError("Index exceeds array length. Array is not modified");
				return array;
			}
			T[] array2 = array.InsertAt(index);
			array2[index] = item;
			return array2;
		}

		public static T[] InsertAt<T>(this T[] array, int index)
		{
			if (index < 0)
			{
				Debug.LogError("Index is less than zero. Array is not modified");
				return array;
			}
			if (index > array.Length)
			{
				Debug.LogError("Index exceeds array length. Array is not modified");
				return array;
			}
			T[] array2 = new T[array.Length + 1];
			int num = 0;
			for (int i = 0; i < array2.Length; i++)
			{
				if (i != index)
				{
					array2[i] = array[num];
					num++;
				}
			}
			return array2;
		}

		public static T[] RemoveAt<T>(this T[] array, int index)
		{
			if (index < 0)
			{
				Debug.LogError("Index is less than zero. Array is not modified");
				return array;
			}
			if (index >= array.Length)
			{
				Debug.LogError("Index exceeds array length. Array is not modified");
				return array;
			}
			T[] array2 = new T[array.Length - 1];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (i != index)
				{
					array2[num] = array[i];
					num++;
				}
			}
			return array2;
		}

		public static T GetRandom<T>(this T[] collection)
		{
			return collection[UnityEngine.Random.Range(0, collection.Length)];
		}

		public static T GetRandom<T>(this IList<T> collection)
		{
			return collection[UnityEngine.Random.Range(0, collection.Count)];
		}

		public static T GetRandom<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
		}

		public static bool IsNullOrEmpty<T>(this T[] collection)
		{
			if (collection != null)
			{
				return collection.Length == 0;
			}
			return true;
		}

		public static bool IsNullOrEmpty<T>(this IList<T> collection)
		{
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return true;
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			if (collection != null)
			{
				return !collection.Any();
			}
			return true;
		}

		public static bool NotNullOrEmpty<T>(this T[] collection)
		{
			return !collection.IsNullOrEmpty();
		}

		public static bool NotNullOrEmpty<T>(this IList<T> collection)
		{
			return !collection.IsNullOrEmpty();
		}

		public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return !collection.IsNullOrEmpty();
		}

		public static int NextIndexInCircle<T>(this T[] array, int desiredPosition)
		{
			if (array.IsNullOrEmpty())
			{
				Debug.LogError("NextIndexInCircle Caused: source array is null or empty");
				return -1;
			}
			int num = array.Length;
			if (num == 1)
			{
				return 0;
			}
			return (desiredPosition % num + num) % num;
		}

		public static int IndexOfItem<T>(this IEnumerable<T> collection, T item)
		{
			if (collection == null)
			{
				Debug.LogError("IndexOfItem Caused: source collection is null");
				return -1;
			}
			int num = 0;
			foreach (T item2 in collection)
			{
				if (object.Equals(item2, item))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static bool ContentsMatch<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			if (first.IsNullOrEmpty() && second.IsNullOrEmpty())
			{
				return true;
			}
			if (first.IsNullOrEmpty() || second.IsNullOrEmpty())
			{
				return false;
			}
			int num = first.Count();
			int num2 = second.Count();
			if (num != num2)
			{
				return false;
			}
			foreach (T item in first)
			{
				if (!second.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		public static bool ContentsMatchKeys<T1, T2>(this IDictionary<T1, T2> source, IEnumerable<T1> check)
		{
			if (source.IsNullOrEmpty() && check.IsNullOrEmpty())
			{
				return true;
			}
			if (source.IsNullOrEmpty() || check.IsNullOrEmpty())
			{
				return false;
			}
			return source.Keys.ContentsMatch(check);
		}

		public static bool ContentsMatchValues<T1, T2>(this IDictionary<T1, T2> source, IEnumerable<T2> check)
		{
			if (source.IsNullOrEmpty() && check.IsNullOrEmpty())
			{
				return true;
			}
			if (source.IsNullOrEmpty() || check.IsNullOrEmpty())
			{
				return false;
			}
			return source.Values.ContentsMatch(check);
		}

		public static TValue GetOrAddDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) where TValue : new()
		{
			if (!source.ContainsKey(key))
			{
				source[key] = new TValue();
			}
			return source[key];
		}

		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
		{
			if (!source.ContainsKey(key))
			{
				source[key] = value;
			}
			return source[key];
		}

		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> valueFactory)
		{
			if (!source.ContainsKey(key))
			{
				source[key] = valueFactory();
			}
			return source[key];
		}

		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> valueFactory)
		{
			if (!source.ContainsKey(key))
			{
				source[key] = valueFactory(key);
			}
			return source[key];
		}

		public static TValue GetOrAdd<TKey, TValue, TArg>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
		{
			if (!source.ContainsKey(key))
			{
				source[key] = valueFactory(key, factoryArgument);
			}
			return source[key];
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T item in source)
			{
				action(item);
			}
			return source;
		}

		public static IEnumerable<T> ForEach<T, R>(this IEnumerable<T> source, Func<T, R> func)
		{
			foreach (T item in source)
			{
				func(item);
			}
			return source;
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			int num = 0;
			foreach (T item in source)
			{
				action(item, num);
				num++;
			}
			return source;
		}

		public static IEnumerable<T> ForEach<T, R>(this IEnumerable<T> source, Func<T, int, R> func)
		{
			int num = 0;
			foreach (T item in source)
			{
				func(item, num);
				num++;
			}
			return source;
		}

		public static T MaxBy<T, S>(this IEnumerable<T> source, Func<T, S> selector) where S : IComparable<S>
		{
			if (source.IsNullOrEmpty())
			{
				Debug.LogError("MaxBy Caused: source collection is null or empty");
				return default(T);
			}
			return source.Aggregate((T e, T n) => (selector(e).CompareTo(selector(n)) <= 0) ? n : e);
		}

		public static T MinBy<T, S>(this IEnumerable<T> source, Func<T, S> selector) where S : IComparable<S>
		{
			if (source.IsNullOrEmpty())
			{
				Debug.LogError("MinBy Caused: source collection is null or empty");
				return default(T);
			}
			return source.Aggregate((T e, T n) => (selector(e).CompareTo(selector(n)) >= 0) ? n : e);
		}

		public static IEnumerable<T> SingleToEnumerable<T>(this T source)
		{
			return Enumerable.Empty<T>().Append(source);
		}

		public static int FirstIndex<T>(this IList<T> source, Predicate<T> predicate)
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (predicate(source[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static int FirstIndex<T>(this IEnumerable<T> source, Predicate<T> predicate)
		{
			int num = 0;
			foreach (T item in source)
			{
				if (predicate(item))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static int LastIndex<T>(this IList<T> source, Predicate<T> predicate)
		{
			for (int num = source.Count - 1; num >= 0; num--)
			{
				if (predicate(source[num]))
				{
					return num;
				}
			}
			return -1;
		}

		public static int GetWeightedRandomIndex<T>(this IEnumerable<T> source, Func<T, double> weightSelector)
		{
			IEnumerable<double> weights = from w in source.Select(weightSelector)
				select (!(w < 0.0)) ? w : 0.0;
			IEnumerable<double> source2 = weights.Select((double w, int i) => weights.Take(i + 1).Sum());
			double roll = MyCommonConstants.SystemRandom.NextDouble() * weights.Sum();
			return source2.FirstIndex((double ws) => ws > roll);
		}

		public static T GetWeightedRandom<T>(this IList<T> source, Func<T, double> weightSelector)
		{
			return source[source.GetWeightedRandomIndex(weightSelector)];
		}

		public static T GetWeightedRandom<T>(this IEnumerable<T> source, Func<T, double> weightSelector)
		{
			return source.ElementAt(source.GetWeightedRandomIndex(weightSelector));
		}

		public static IList<T> FillBy<T>(this IList<T> source, Func<int, T> valueFactory)
		{
			for (int i = 0; i < source.Count; i++)
			{
				source[i] = valueFactory(i);
			}
			return source;
		}

		public static T[] FillBy<T>(this T[] source, Func<int, T> valueFactory)
		{
			for (int i = 0; i < source.Length; i++)
			{
				source[i] = valueFactory(i);
			}
			return source;
		}

		public static T[] ExclusiveSample<T>(this IList<T> source, int sampleNumber)
		{
			if (sampleNumber > source.Count)
			{
				throw new ArgumentOutOfRangeException("Cannot sample more elements than what the source collection contains");
			}
			T[] array = new T[sampleNumber];
			int num = 0;
			for (int i = 0; i < source.Count; i++)
			{
				if (num >= sampleNumber)
				{
					break;
				}
				double num2 = (double)(sampleNumber - num) / (double)(source.Count - i);
				if (MyCommonConstants.SystemRandom.NextDouble() < num2)
				{
					array[num] = source[i];
					num++;
				}
			}
			return array;
		}

		public static IList<T> SwapInPlace<T>(this IList<T> source, int index1, int index2)
		{
			T val = source[index2];
			T val2 = source[index1];
			T val3 = (source[index1] = val);
			val3 = (source[index2] = val2);
			return source;
		}

		public static IList<T> Shuffle<T>(this IList<T> source)
		{
			for (int i = 0; i < source.Count - 1; i++)
			{
				int index = UnityEngine.Random.Range(i, source.Count);
				source.SwapInPlace(i, index);
			}
			return source;
		}
	}
}
