using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class ListExtensions
	{
		public static T Pop<T>(this IList<T> list, int index)
		{
			T result = list[index];
			list.RemoveAt(index);
			return result;
		}

		public static List<T> Pop<T>(this IList<T> list, params int[] indexes)
		{
			List<T> list2 = new List<T>();
			foreach (int index in indexes)
			{
				list2.Add(list.Pop(index));
			}
			return list2;
		}

		public static (T element, int index) PopRandom<T>(this IList<T> list)
		{
			int num = UnityEngine.Random.Range(0, list.Count);
			return (element: list.Pop(num), index: num);
		}

		public static List<(T element, int index)> PopRandoms<T>(this IList<T> list, int count)
		{
			List<(T, int)> list2 = new List<(T, int)>();
			for (int i = 0; i < count; i++)
			{
				list2.Add(list.PopRandom());
			}
			return list2;
		}

		public static (T element, int index) PopRandomElementWithProbability<T>(this IList<T> list, params float[] probabilities)
		{
			return list.PopRandomElementWithProbability((IEnumerable<float>)probabilities);
		}

		public static (T element, int index) PopRandomElementWithProbability<T>(this IList<T> list, IEnumerable<float> probabilities)
		{
			(T, int) randomElementWithProbability = list.GetRandomElementWithProbability(probabilities);
			list.Pop(randomElementWithProbability.Item2);
			return randomElementWithProbability;
		}

		public static (T element, int index) PopRandomElementWithProbability<T>(this IList<T> list, Func<T, float> probabilitiesSelector)
		{
			(T, int) randomElementWithProbability = list.GetRandomElementWithProbability(probabilitiesSelector);
			list.Pop(randomElementWithProbability.Item2);
			return randomElementWithProbability;
		}

		public static void RemoveRange<T>(this IList<T> list, int index)
		{
			for (int i = list.Count - 1; i >= index; i++)
			{
				list.RemoveAt(i);
			}
		}
	}
}
