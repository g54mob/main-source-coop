using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class EnumerableExtensions
	{
		public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Count()));
		}

		public static List<T> GetRandomElements<T>(this IEnumerable<T> enumerable, int count)
		{
			IEnumerable<int> poppedIndexes = from p in Enumerable.Range(0, enumerable.Count()).ToList().PopRandoms(count)
				select p.index;
			return enumerable.Where((T el, int i) => poppedIndexes.Contains(i)).ToList();
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] elements)
		{
			return Enumerable.Except(enumerable, elements);
		}

		public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.OrderBy((T v) => UnityEngine.Random.value);
		}

		public static string AsString<T>(this IEnumerable<T> enumerable)
		{
			return "[" + string.Join(", ", enumerable) + "]";
		}

		public static (T element, int index) GetRandomElementWithProbability<T>(this IEnumerable<T> enumerable, params float[] probabilities)
		{
			return enumerable.GetRandomElementWithProbability((IEnumerable<float>)probabilities);
		}

		public static (T element, int index) GetRandomElementWithProbability<T>(this IEnumerable<T> enumerable, IEnumerable<float> probabilities)
		{
			int num = enumerable.Count();
			if (probabilities.Count() != num)
			{
				throw new ArgumentException("Count of probabilities and enumerble elements must be equal.");
			}
			if (num == 0)
			{
				throw new ArgumentException("Enumerable count must be greater than zero");
			}
			float num2 = UnityEngine.Random.value * probabilities.Sum();
			float num3 = 0f;
			int num4 = -1;
			IEnumerator<float> enumerator = probabilities.GetEnumerator();
			while (enumerator.MoveNext())
			{
				num4++;
				float current = enumerator.Current;
				num3 += current;
				if (num2 < num3 || num2.Approximately(num3))
				{
					return (element: enumerable.ElementAt(num4), index: num4);
				}
			}
			num4 = probabilities.Count() - 1;
			return (element: enumerable.ElementAt(num4), index: num4);
		}

		public static (T element, int index) GetRandomElementWithProbability<T>(this IEnumerable<T> enumerable, Func<T, float> probabilitySelector)
		{
			return enumerable.GetRandomElementWithProbability(enumerable.Select((T el) => probabilitySelector(el)));
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (T item in enumerable)
			{
				action(item);
			}
		}
	}
}
