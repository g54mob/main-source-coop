using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	public static class ArrayTools
	{
		public static void RemoveAt<T>(ref T[] array, int num)
		{
			array = RemoveAt(array, num);
		}

		public static T[] RemoveAt<T>(T[] array, int num)
		{
			if (num >= array.Length || num < 0)
			{
				num = array.Length - 1;
			}
			T[] array2 = new T[array.Length - 1];
			if (num != 0)
			{
				Array.Copy(array, array2, num);
			}
			if (num != array.Length)
			{
				Array.Copy(array, num + 1, array2, num, array2.Length - num);
			}
			return array2;
		}

		public static void Remove<T>(ref T[] array, T obj) where T : class
		{
			array = Remove(array, obj);
		}

		public static T[] Remove<T>(T[] array, T obj) where T : class
		{
			int num = array.Find(obj);
			return RemoveAt(array, num);
		}

		public static void Add<T>(ref T[] array, T element)
		{
			array = Add(array, element);
		}

		public static T[] Add<T>(T[] array, T element)
		{
			if (array == null || array.Length == 0)
			{
				return new T[1] { element };
			}
			T[] array2 = new T[array.Length + 1];
			Array.Copy(array, array2, array.Length);
			array2[array.Length] = element;
			return array2;
		}

		public static void Add<T>(ref T[] array, T element1, T element2)
		{
			array = Add(array, element1, element2);
		}

		public static T[] Add<T>(T[] array, T element1, T element2)
		{
			if (array == null || array.Length == 0)
			{
				return new T[2] { element1, element2 };
			}
			T[] array2 = new T[array.Length + 2];
			Array.Copy(array, array2, array.Length);
			array2[array.Length] = element1;
			array2[array.Length + 1] = element2;
			return array2;
		}

		public static void Add<T>(ref T[] array, T element1, T element2, T element3)
		{
			array = Add(array, element1, element2, element3);
		}

		public static T[] Add<T>(T[] array, T element1, T element2, T element3)
		{
			if (array == null || array.Length == 0)
			{
				return new T[3] { element1, element2, element3 };
			}
			T[] array2 = new T[array.Length + 3];
			Array.Copy(array, array2, array.Length);
			array2[array.Length] = element1;
			array2[array.Length + 1] = element2;
			array2[array.Length + 2] = element3;
			return array2;
		}

		public static void AddRange<T>(ref T[] array, T[] other)
		{
			array = AddRange(array, other);
		}

		public static T[] AddRange<T>(T[] array, T[] other)
		{
			if (array == null || array.Length == 0)
			{
				T[] array2 = new T[other.Length];
				Array.Copy(other, array2, other.Length);
				return array2;
			}
			T[] array3 = new T[array.Length + other.Length];
			Array.Copy(array, array3, array.Length);
			Array.Copy(other, 0, array3, array.Length, other.Length);
			return array3;
		}

		public static void AddLayer<T>(ref T[,,] array, T[,,] otherArray, int channel)
		{
			array = AddLayer(array, otherArray, channel);
		}

		public static T[,,] AddLayer<T>(T[,,] array, T[,,] otherArray, int channel)
		{
			int length = array.GetLength(0);
			int length2 = array.GetLength(1);
			int length3 = array.GetLength(2);
			T[,,] array2 = new T[length, length2, length3 + 1];
			Array.Copy(array, array2, length * length2 * length3);
			CopyLayer(otherArray, array2, channel, length3);
			return array2;
		}

		public static void Insert<T>(ref T[] array, int pos, T element)
		{
			array = Insert(array, pos, element);
		}

		public static void Insert<T>(ref T[] array, int pos, Func<int, T> createElement)
		{
			array = Insert(array, pos, createElement(pos));
		}

		public static T[] Insert<T>(T[] array, int pos, T element)
		{
			if (array == null || array.Length == 0)
			{
				return new T[1] { element };
			}
			if (pos > array.Length || pos < 0)
			{
				pos = array.Length;
			}
			T[] array2 = new T[array.Length + 1];
			if (pos != 0)
			{
				Array.Copy(array, array2, pos);
			}
			if (pos != array.Length)
			{
				Array.Copy(array, pos, array2, pos + 1, array.Length - pos);
			}
			array2[pos] = element;
			return array2;
		}

		public static void InsertRemoveLast<T>(this T[] array, int pos, T element)
		{
			Array.Copy(array, pos, array, pos + 1, array.Length - pos - 1);
			array[pos] = element;
		}

		public static T[] InsertRange<T>(T[] array, int after, T[] add)
		{
			if (after > array.Length || after < 0)
			{
				after = array.Length;
			}
			T[] array2 = new T[array.Length + add.Length];
			if (after != 0)
			{
				Array.Copy(array, array2, after);
			}
			Array.Copy(add, 0, array2, after, add.Length);
			if (after != array.Length)
			{
				Array.Copy(array, after, array2, after + add.Length, array.Length - after);
			}
			return array2;
		}

		public static void Resize<T>(ref T[] array, int newSize, Func<int, T> createElement = null)
		{
			array = Resize(array, newSize, createElement);
		}

		public static T[] Resize<T>(T[] array, int newSize, Func<int, T> createElement = null)
		{
			T[] array2 = new T[newSize];
			Array.Copy(array, array2, (newSize < array.Length) ? newSize : array.Length);
			if (newSize > array.Length && createElement != null)
			{
				for (int i = array.Length; i < newSize; i++)
				{
					array2[i] = createElement(i);
				}
			}
			return array2;
		}

		public static void Resize<T>(ref T[] array, int newSize, T def)
		{
			array = Resize(array, newSize, def);
		}

		public static T[] Resize<T>(T[] array, int newSize, T def)
		{
			T[] array2 = new T[newSize];
			Array.Copy(array, array2, (newSize < array.Length) ? newSize : array.Length);
			if (newSize > array.Length)
			{
				for (int i = array.Length; i < newSize; i++)
				{
					array2[i] = def;
				}
			}
			return array2;
		}

		public static void ResizeLayers<T>(ref T[,,] array, int newSize, T def)
		{
			array = ResizeLayers(array, newSize, def);
		}

		public static T[,,] ResizeLayers<T>(T[,,] array, int newSize, T def)
		{
			int length = array.GetLength(2);
			int length2 = array.GetLength(0);
			int length3 = array.GetLength(1);
			T[,,] array2 = new T[array.GetLength(0), array.GetLength(1), newSize];
			for (int i = 0; i < length2; i++)
			{
				for (int j = 0; j < length3; j++)
				{
					for (int k = 0; k < newSize; k++)
					{
						T val = ((k < length) ? array[i, j, k] : def);
						array2[i, j, k] = val;
					}
				}
			}
			return array2;
		}

		public static void CopyLayer<T>(T[,,] src, T[,,] dst, int srcNum, int dstNum)
		{
			int length = src.GetLength(0);
			int length2 = src.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					dst[i, j, dstNum] = src[i, j, srcNum];
				}
			}
		}

		public static void Append<T>(ref T[] array, T[] additional)
		{
			array = Append(array, additional);
		}

		public static T[] Append<T>(T[] array, T[] additional)
		{
			T[] array2 = new T[array.Length + additional.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			for (int j = 0; j < additional.Length; j++)
			{
				array2[j + array.Length] = additional[j];
			}
			return array2;
		}

		public static void Switch<T>(T[] array, int num1, int num2)
		{
			if (num1 >= 0 && num1 < array.Length && num2 >= 0 && num2 < array.Length)
			{
				T val = array[num1];
				array[num1] = array[num2];
				array[num2] = val;
			}
		}

		public static void Switch<T>(T[] array, T obj1, T obj2) where T : class
		{
			int num = array.Find(obj1);
			int num2 = array.Find(obj2);
			Switch(array, num, num2);
		}

		public static void Replace<T>(this T[] array, T obj1, T obj2) where T : IEquatable<T>
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(obj1))
				{
					array[i] = obj2;
				}
			}
		}

		public static void ReplaceNum<T>(this T[] array, int n1, int n2) where T : IEquatable<T>
		{
			T other = array[n1];
			T val = array[n2];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(other))
				{
					array[i] = val;
				}
			}
		}

		public static void Move<T>(T[] array, int src, int dst)
		{
			T val = array[src];
			if (src < dst)
			{
				for (int i = src; i < dst; i++)
				{
					array[i] = array[i + 1];
				}
			}
			if (src > dst)
			{
				for (int num = src; num > dst; num--)
				{
					array[num] = array[num - 1];
				}
			}
			array[dst] = val;
		}

		public static T[] Truncated<T>(this T[] src, int length)
		{
			T[] array = new T[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = src[i];
			}
			return array;
		}

		public static bool Equals<T>(T[] a1, T[] a2) where T : class
		{
			if (a1.Length != a2.Length)
			{
				return false;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				if (a1[i] != a2[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool EqualsEquatable<T>(T[] a1, T[] a2) where T : IEquatable<T>
		{
			if (a1.Length != a2.Length)
			{
				return false;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				if (!object.Equals(a1[i], a2[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool EqualsVector3(Vector3[] a1, Vector3[] a2, float delta = 1E-45f)
		{
			if (a1 == null || a2 == null || a1.Length != a2.Length)
			{
				return false;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				float num = a1[i].x - a2[i].x;
				if (!(num < delta) || !(0f - num < delta))
				{
					return false;
				}
				num = a1[i].y - a2[i].y;
				if (!(num < delta) || !(0f - num < delta))
				{
					return false;
				}
				num = a1[i].z - a2[i].z;
				if (!(num < delta) || !(0f - num < delta))
				{
					return false;
				}
			}
			return true;
		}

		public static int Find<T>(this T[] array, T obj)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (object.Equals(array[i], obj))
				{
					return i;
				}
			}
			return -1;
		}

		public static int Find<T>(this T[] array, Predicate<T> func)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (func(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static T FindMember<T>(this T[] array, Func<T, bool> func) where T : class
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (func(array[i]))
				{
					return array[i];
				}
			}
			return null;
		}

		public static int FindCount<T>(this T[] array, T obj)
		{
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (object.Equals(array[i], obj))
				{
					num++;
				}
			}
			return num;
		}

		public static List<int> FindAllIndexes<T>(this T[] array, Func<T, bool> func)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (func(array[i]))
				{
					list.Add(i);
				}
			}
			return list;
		}

		public static TT FindMemberOfType<T, TT>(this T[] array)
		{
			foreach (T val in array)
			{
				if (val is TT)
				{
					return (TT)((((object)val) is TT) ? ((object)val) : null);
				}
			}
			return default(TT);
		}

		public static T FindSmallest<T>(this T[] array, Func<T, float> func)
		{
			if (array.Length == 0)
			{
				return default(T);
			}
			if (array.Length == 1)
			{
				return array[0];
			}
			float num = 3.4028235E+38f;
			int num2 = -1;
			for (int i = 0; i < array.Length; i++)
			{
				float num3 = func(array[i]);
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
			return array[num2];
		}

		public static T FindBiggest<T>(this T[] array, Func<T, float> func)
		{
			if (array.Length == 0)
			{
				return default(T);
			}
			if (array.Length == 1)
			{
				return array[0];
			}
			float num = -3.4028235E+38f;
			int num2 = -1;
			for (int i = 0; i < array.Length; i++)
			{
				float num3 = func(array[i]);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			return array[num2];
		}

		public static bool Contains<T>(this T[] array, T obj)
		{
			if (array == null)
			{
				return false;
			}
			if (Array.IndexOf(array, obj) >= 0)
			{
				return true;
			}
			return false;
		}

		public static bool Contains<T>(this T[] array, Predicate<T> func)
		{
			if (array == null)
			{
				return false;
			}
			if (array.Find(func) >= 0)
			{
				return true;
			}
			return false;
		}

		public static bool ContainsNull<T>(this T[] array) where T : class
		{
			if (array == null)
			{
				return false;
			}
			if (Array.IndexOf(array, null) >= 0)
			{
				return true;
			}
			return false;
		}

		public static bool Intersects<T>(T[] array1, T[] array2) where T : class
		{
			foreach (T val in array1)
			{
				foreach (T val2 in array2)
				{
					if (val == val2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool AllNull<T>(this T[] array) where T : class
		{
			if (array == null)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		public static T Any<T>(this T[] array) where T : class
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					return array[i];
				}
			}
			return null;
		}

		public static int Max(this int[] array)
		{
			int num = -2147483648;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > num)
				{
					num = array[i];
				}
			}
			return num;
		}

		public static bool Empty<T>(this T[] array) where T : class
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		public static void RemoveAll<T>(ref T[] array, T obj) where T : class
		{
			array = RemoveAll(array, obj);
		}

		public static T[] RemoveAll<T>(T[] array, T obj) where T : class
		{
			bool[] array2 = new bool[array.Length];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (object.Equals(array[i], obj))
				{
					num++;
					array2[i] = true;
				}
			}
			T[] array3 = new T[array.Length - num];
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (!array2[j])
				{
					array3[num2] = array[j];
					num2++;
				}
			}
			return array3;
		}

		public static void RemoveAllFunc<T>(ref T[] array, Func<T, bool> func) where T : class
		{
			array = RemoveAllFunc(array, func);
		}

		public static T[] RemoveAllFunc<T>(T[] array, Func<T, bool> func) where T : class
		{
			bool[] array2 = new bool[array.Length];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (func(array[i]))
				{
					num++;
					array2[i] = true;
				}
			}
			T[] array3 = new T[array.Length - num];
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (!array2[j])
				{
					array3[num2] = array[j];
					num2++;
				}
			}
			return array3;
		}

		public static T[] RemoveNulls<T>(this T[] array) where T : class
		{
			if (array == null)
			{
				return array;
			}
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return array;
			}
			if (num == array.Length)
			{
				return new T[0];
			}
			T[] array2 = new T[array.Length - num];
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					array2[num2] = array[j];
					num2++;
				}
			}
			return array2;
		}

		public static bool MatchExactly<T>(T[] arr1, T[] arr2)
		{
			if (arr1.Length != arr2.Length)
			{
				return false;
			}
			for (int i = 0; i < arr1.Length; i++)
			{
				if (!object.Equals(arr1[i], arr2[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool MatchElements<T>(T[] arr1, T[] arr2)
		{
			if (arr1.Length != arr2.Length)
			{
				return false;
			}
			bool[] array = new bool[arr1.Length];
			for (int i = 0; i < arr1.Length; i++)
			{
				for (int j = 0; j < arr2.Length; j++)
				{
					if (!array[j] && object.Equals(arr1[i], arr2[j]))
					{
						array[j] = true;
						break;
					}
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (!array[k])
				{
					return false;
				}
			}
			return true;
		}

		public static void AddIfNotContains<T>(ref T[] array, T element)
		{
			array = AddIfNotContains(array, element);
		}

		public static T[] AddIfNotContains<T>(T[] array, T element)
		{
			if (array.FindCount(element) == 0)
			{
				return Add(array, element);
			}
			return array;
		}

		public static void AddRangeIfNotContains<T>(ref T[] array, IEnumerable<T> add)
		{
			array = AddRangeIfNotContains(array, add);
		}

		public static T[] AddRangeIfNotContains<T>(T[] array, IEnumerable<T> add)
		{
			List<T> list = new List<T>();
			foreach (T item in add)
			{
				if (!array.Contains(item))
				{
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				return array;
			}
			return AddRange(array, list.ToArray());
		}

		public static bool IsEmpty<T>(T[] array) where T : class
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		public static void Rewrite<T>(List<T> src, ref T[] dst)
		{
			if (dst.Length != src.Count)
			{
				dst = new T[src.Count];
			}
			for (int i = 0; i < dst.Length; i++)
			{
				dst[i] = src[i];
			}
		}

		public static void Rewrite<T1, T2>(List<T1> src, ref T2[] dst, Func<T1, T2> fn)
		{
			if (dst.Length != src.Count)
			{
				dst = new T2[src.Count];
			}
			for (int i = 0; i < dst.Length; i++)
			{
				dst[i] = fn(src[i]);
			}
		}

		public static T[] Process<T>(this T[] arr, Func<int, T> func)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = func(i);
			}
			return arr;
		}

		public static TDst[] Select<TSrc, TDst>(this TSrc[] src, Func<TSrc, TDst> fn)
		{
			TDst[] array = new TDst[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				array[i] = fn(src[i]);
			}
			return array;
		}

		public static T[] Convert<T, Y>(Y[] src)
		{
			T[] array = new T[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				array[i] = (T)(object)src[i];
			}
			return array;
		}

		public static void Convert<T, Y>(Y[] src, T[] dst)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i] = (T)(object)src[i];
			}
		}

		public static void Convert<T, Y>(Y[] src, T[] dst, Func<Y, T> convertFn)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i] = convertFn(src[i]);
			}
		}

		public static T[] Convert<T, Y>(ICollection<Y> src)
		{
			Y[] array = new Y[src.Count];
			src.CopyTo(array, 0);
			return Convert<T, Y>(array);
		}

		public static void FillNulls<T>(this T[] arr, Func<T> func) where T : class
		{
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] == null)
				{
					arr[i] = func();
				}
			}
		}

		public static void Fill<T>(this T[] arr, T val)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = val;
			}
		}

		public static string ToStringMemberwise(this Array array, string separator = ", ")
		{
			string text = "";
			if (array.Length == 0)
			{
				return text;
			}
			for (int i = 0; i < array.Length; i++)
			{
				object value = array.GetValue(i);
				text += ((value != null) ? value.ToString() : "");
				if (i != array.Length - 1)
				{
					text += separator;
				}
			}
			return text;
		}

		public static void RandomMix<T>(this T[] array, int iterations = 2)
		{
			for (int i = 0; i < iterations; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					int num = UnityEngine.Random.Range(0, array.Length);
					if (j != num)
					{
						T val = array[num];
						array[num] = array[j];
						array[j] = val;
					}
				}
			}
		}

		public static T Last<T>(this T[] array) where T : class
		{
			if (array.Length == 0)
			{
				return null;
			}
			return array[array.Length - 1];
		}

		public static T[] Copy<T>(this T[] array)
		{
			T[] array2 = new T[array.Length];
			Array.Copy(array, array2, array.Length);
			return array2;
		}

		public static T[][] CopyJagged<T>(this T[][] array)
		{
			T[][] array2 = new T[array.Length][];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].Copy();
			}
			return array2;
		}

		public static TA[] WhereSelect<TC, TA>(this ICollection<TC> collection, Func<TC, TA> fn)
		{
			TA[] array = new TA[collection.Count];
			int num = 0;
			foreach (TC item in collection)
			{
				array[num] = fn(item);
				num++;
			}
			return array;
		}

		public static void QSort(float[] array)
		{
			QSort(array, 0, array.Length - 1);
		}

		public static void QSort(float[] array, int l, int r)
		{
			float num = array[l + (r - l) / 2];
			int i = l;
			int num2 = r;
			while (i <= num2)
			{
				for (; array[i] < num; i++)
				{
				}
				while (array[num2] > num)
				{
					num2--;
				}
				if (i <= num2)
				{
					float num3 = array[i];
					array[i] = array[num2];
					array[num2] = num3;
					i++;
					num2--;
				}
			}
			if (i < r)
			{
				QSort(array, i, r);
			}
			if (l < num2)
			{
				QSort(array, l, num2);
			}
		}

		public static void QSort<T>(T[] array, float[] reference)
		{
			QSort(array, reference, 0, reference.Length - 1);
		}

		public static void QSort<T>(T[] array, float[] reference, int l, int r)
		{
			float num = reference[l + (r - l) / 2];
			int i = l;
			int num2 = r;
			while (i <= num2)
			{
				for (; reference[i] < num; i++)
				{
				}
				while (reference[num2] > num)
				{
					num2--;
				}
				if (i <= num2)
				{
					float num3 = reference[i];
					reference[i] = reference[num2];
					reference[num2] = num3;
					T val = array[i];
					array[i] = array[num2];
					array[num2] = val;
					i++;
					num2--;
				}
			}
			if (i < r)
			{
				QSort(array, reference, i, r);
			}
			if (l < num2)
			{
				QSort(array, reference, l, num2);
			}
		}

		public static void QSort<T>(List<T> list, float[] reference)
		{
			QSort(list, reference, 0, reference.Length - 1);
		}

		public static void QSort<T>(List<T> list, float[] reference, int l, int r)
		{
			float num = reference[l + (r - l) / 2];
			int i = l;
			int num2 = r;
			while (i <= num2)
			{
				for (; reference[i] < num; i++)
				{
				}
				while (reference[num2] > num)
				{
					num2--;
				}
				if (i <= num2)
				{
					float num3 = reference[i];
					reference[i] = reference[num2];
					reference[num2] = num3;
					T value = list[i];
					list[i] = list[num2];
					list[num2] = value;
					i++;
					num2--;
				}
			}
			if (i < r)
			{
				QSort(list, reference, i, r);
			}
			if (l < num2)
			{
				QSort(list, reference, l, num2);
			}
		}

		public static int[] Order(int[] array, int[] order = null, int max = 0, int steps = 1000000, int[] stepsArray = null)
		{
			if (max == 0)
			{
				max = array.Length;
			}
			if (stepsArray == null)
			{
				stepsArray = new int[steps + 1];
			}
			else
			{
				steps = stepsArray.Length - 1;
			}
			int[] array2 = new int[steps + 1];
			for (int i = 0; i < max; i++)
			{
				array2[array[i]]++;
			}
			int num = 0;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] += num;
				num = array2[j];
			}
			for (int num2 = array2.Length - 1; num2 > 0; num2--)
			{
				array2[num2] = array2[num2 - 1];
			}
			array2[0] = 0;
			if (order == null)
			{
				order = new int[max];
			}
			for (int k = 0; k < max; k++)
			{
				int num3 = array[k];
				int num4 = array2[num3];
				order[num4] = k;
				array2[num3]++;
			}
			return order;
		}
	}
}
