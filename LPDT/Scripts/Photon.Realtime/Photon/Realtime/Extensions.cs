using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Client;

namespace Photon.Realtime
{
	public static class Extensions
	{
		private static readonly List<object> keysWithNullValue = new List<object>();

		public static int GetStableHashCode(this string str)
		{
			int num = 5381;
			int num2 = num;
			for (int i = 0; i < str.Length && str[i] != 0; i += 2)
			{
				num = ((num << 5) + num) ^ str[i];
				if (i == str.Length - 1 || str[i + 1] == '\0')
				{
					break;
				}
				num2 = ((num2 << 5) + num2) ^ str[i + 1];
			}
			return num + num2 * 1566083941;
		}

		public static string ToStringFull(this PhotonHashtable origin)
		{
			return SupportClass.DictionaryToString(origin, includeTypes: false);
		}

		public static string ToStringFull<T>(this List<T> data)
		{
			if (data == null)
			{
				return "null";
			}
			string[] array = new string[data.Count];
			for (int i = 0; i < data.Count; i++)
			{
				object obj = data[i];
				array[i] = ((obj != null) ? obj.ToString() : "null");
			}
			return string.Join(", ", array);
		}

		public static string ToStringFull(this byte[] list, int count = -1)
		{
			return SupportClass.ByteArrayToString(list, count);
		}

		public static string ToStringFull(this ArraySegment<byte> segment, int count = -1)
		{
			if (count < 0 || count > segment.Count)
			{
				count = segment.Count;
			}
			return BitConverter.ToString(segment.Array, segment.Offset, count);
		}

		public static string ToStringFull(this object[] data)
		{
			if (data == null)
			{
				return "null";
			}
			string[] array = new string[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				object obj = data[i];
				array[i] = ((obj != null) ? obj.ToString() : "null");
			}
			return string.Join(", ", array);
		}

		public static bool CustomPropKeyTypesValid(this PhotonHashtable original, bool NullOrZeroAccepted = false)
		{
			if (original == null || original.Count == 0)
			{
				return NullOrZeroAccepted;
			}
			foreach (DictionaryEntry item in original)
			{
				if (!(item.Key is string) && !(item.Key is int))
				{
					return false;
				}
			}
			return true;
		}

		public static bool CustomPropKeyTypesValid(this object[] array, bool NullOrZeroAccepted = false)
		{
			if (array == null || array.Length == 0)
			{
				return NullOrZeroAccepted;
			}
			foreach (object obj in array)
			{
				if (!(obj is string) && !(obj is int))
				{
					return false;
				}
			}
			return true;
		}

		public static void StripKeysWithNullValues(this PhotonHashtable original)
		{
			lock (keysWithNullValue)
			{
				keysWithNullValue.Clear();
				foreach (DictionaryEntry item in original)
				{
					if (item.Value == null)
					{
						keysWithNullValue.Add(item.Key);
					}
				}
				for (int i = 0; i < keysWithNullValue.Count; i++)
				{
					object key = keysWithNullValue[i];
					original.Remove(key);
				}
			}
		}

		public static void Merge(this PhotonHashtable target, PhotonHashtable addHash)
		{
			if (addHash == null || target.Equals(addHash))
			{
				return;
			}
			foreach (object key in addHash.Keys)
			{
				target[key] = addHash[key];
			}
		}

		public static void MergeStringKeys(this PhotonHashtable target, PhotonHashtable addHash)
		{
			if (addHash == null || target.Equals(addHash))
			{
				return;
			}
			foreach (DictionaryEntry item in addHash)
			{
				if (item.Key is string)
				{
					target[item.Key] = item.Value;
				}
			}
		}

		public static bool Contains(this int[] target, int nr)
		{
			if (target == null)
			{
				return false;
			}
			for (int i = 0; i < target.Length; i++)
			{
				if (target[i] == nr)
				{
					return true;
				}
			}
			return false;
		}
	}
}
