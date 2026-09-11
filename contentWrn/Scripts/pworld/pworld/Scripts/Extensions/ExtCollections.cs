using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtCollections
	{
		public static bool IsInRange<T>(this IEnumerable<T> me, int value)
		{
			if (value >= 0)
			{
				return value < me.Count();
			}
			return false;
		}

		public static bool IsInRange<T>(this int me, IEnumerable<T> list)
		{
			if (me >= 0)
			{
				return me < list.Count();
			}
			return false;
		}

		public static T GetRandom<T>(this T[] me)
		{
			return me[UnityEngine.Random.Range(0, me.Length)];
		}

		public static List<T> GetRandomNoDuplicates<T>(this IEnumerable<T> me, int count)
		{
			List<T> list = new List<T>();
			List<T> list2 = me.ToList();
			for (int i = 0; i < count; i++)
			{
				int index = UnityEngine.Random.Range(0, list2.Count);
				list.Add(list2[index]);
				list2.RemoveAt(index);
			}
			return list;
		}

		public static Dictionary<T, int> GetLedgerOfChances<T>(this List<T> me, Func<T, float> weightFunc, int times = 100)
		{
			Dictionary<T, int> dictionary = new Dictionary<T, int>();
			for (int i = 0; i < times; i++)
			{
				T weightedRandom = me.GetWeightedRandom(weightFunc);
				if (!dictionary.ContainsKey(weightedRandom))
				{
					dictionary.Add(weightedRandom, 0);
				}
				dictionary[weightedRandom]++;
			}
			return dictionary;
		}

		public static void PrintLedger<T>(this Dictionary<T, int> me, int times, Action<T, int> printAction = null)
		{
			List<KeyValuePair<T, int>> list = me.ToList();
			list.Sort((KeyValuePair<T, int> pair1, KeyValuePair<T, int> pair2) => pair1.Value.CompareTo(pair2.Value));
			foreach (var (val2, num2) in list)
			{
				if (printAction == null)
				{
					Debug.Log($"{val2} : {(float)num2 / (float)times}");
				}
				else
				{
					printAction(val2, num2);
				}
			}
		}

		public static T GetWeightedRandom<T>(this List<T> me, Func<T, float> weightFunc)
		{
			float num = 0f;
			foreach (T item in me)
			{
				num += weightFunc(item);
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			foreach (T item2 in me)
			{
				num2 -= weightFunc(item2);
				if (num2 <= 0f)
				{
					return item2;
				}
			}
			throw new Exception("problems");
		}

		public static List<T> PToList<T>(this T me)
		{
			return new List<T> { me };
		}

		public static int PGetRandomIndex<T>(this List<T> me)
		{
			return UnityEngine.Random.Range(0, me.Count);
		}

		public static void PPushRange<T>(this Stack<T> source, IEnumerable<T> collection)
		{
			foreach (T item in collection)
			{
				source.Push(item);
			}
		}

		public static T[][] PToJaggedArray<T>(this T[,] twoDimensionalArray)
		{
			int lowerBound = twoDimensionalArray.GetLowerBound(0);
			int upperBound = twoDimensionalArray.GetUpperBound(0);
			int num = upperBound + 1;
			int lowerBound2 = twoDimensionalArray.GetLowerBound(1);
			int upperBound2 = twoDimensionalArray.GetUpperBound(1);
			int num2 = upperBound2 + 1;
			T[][] array = new T[num][];
			for (int i = lowerBound; i <= upperBound; i++)
			{
				array[i] = new T[num2];
				for (int j = lowerBound2; j <= upperBound2; j++)
				{
					array[i][j] = twoDimensionalArray[i, j];
				}
			}
			return array;
		}

		public static T FindClosest<T>(this Vector3 pos, List<T> list) where T : MonoBehaviour
		{
			if (list.Count < 1)
			{
				return null;
			}
			T result = null;
			float num = float.PositiveInfinity;
			foreach (T item in list)
			{
				float num2 = Vector3.Distance(pos, item.transform.position);
				if (num2 < num)
				{
					result = item;
					num = num2;
				}
			}
			return result;
		}

		public static Transform FindClosest(this List<Transform> list, Vector3 position)
		{
			float num = float.MaxValue;
			Transform result = null;
			foreach (Transform item in list)
			{
				float num2 = Vector3.Distance(position, item.position);
				if (num2 < num)
				{
					num = num2;
					result = item;
				}
			}
			return result;
		}

		public static T FindClosest<T>(this List<T> list, Vector3 position) where T : MonoBehaviour
		{
			float num = float.MaxValue;
			T result = null;
			foreach (T item in list)
			{
				float num2 = Vector3.Distance(position, item.transform.position);
				if (num2 < num)
				{
					num = num2;
					result = item;
				}
			}
			return result;
		}

		public static T GetFromBack<T>(this List<T> me, int nrFromBack = 0)
		{
			return me[me.Count - nrFromBack];
		}

		public static void ForEachBackwards<T>(this List<T> me, Action<T> doIt)
		{
			for (int num = me.Count - 1; num >= 0; num--)
			{
				doIt(me[num]);
			}
		}

		public static T GetRnd<T>(this List<T> me)
		{
			return me[UnityEngine.Random.Range(0, me.Count)];
		}

		public static IList<T> Shuffle<T>(this IList<T> list)
		{
			int num = list.Count;
			while (num > 1)
			{
				num--;
				int index = UnityEngine.Random.Range(0, num + 1);
				T value = list[index];
				list[index] = list[num];
				list[num] = value;
			}
			return list;
		}

		public static void AddRange<T>(this ObservableCollection<T> me, IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				me.Add(item);
			}
		}

		public static bool Contains<T>(this HashSet<T> me, List<T> list)
		{
			foreach (T item in list)
			{
				if (!me.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		public static (T[] first, T[] second) Split<T>(this T[] me, int split)
		{
			if (split >= me.Length)
			{
				throw new Exception("pick a reasonable split point");
			}
			T[] array = new T[split];
			T[] array2 = new T[me.Length - split];
			for (int i = 0; i < me.Length; i++)
			{
				if (i < split)
				{
					array[i] = me[i];
				}
				else
				{
					array2[i - split] = me[i];
				}
			}
			return (first: array, second: array2);
		}
	}
}
