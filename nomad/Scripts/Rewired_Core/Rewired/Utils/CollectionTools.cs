using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal static class CollectionTools
	{
		public static Dictionary<TValue, TKey> CreateInverseDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
		{
			if (dict == null)
			{
				return null;
			}
			Dictionary<TValue, TKey> dictionary = new Dictionary<TValue, TKey>();
			foreach (KeyValuePair<TKey, TValue> item in dict)
			{
				if (!dictionary.ContainsKey(item.Value))
				{
					dictionary.Add(item.Value, item.Key);
				}
			}
			return dictionary;
		}

		public static TReturn GetDictionaryValueSafe<TReturn>(Dictionary<string, object> dictionary, string key)
		{
			bool success;
			return GetDictionaryValueSafe<TReturn>(dictionary, key, out success);
		}

		public static TReturn GetDictionaryValueSafe<TReturn>(Dictionary<string, object> dictionary, string key, out bool success)
		{
			success = false;
			if (dictionary == null)
			{
				return default(TReturn);
			}
			if (!dictionary.TryGetValue(key, out var value))
			{
				return default(TReturn);
			}
			if (!(value is TReturn))
			{
				return default(TReturn);
			}
			success = true;
			return (TReturn)value;
		}

		public static TValue GetDictionaryValueSafe<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
		{
			bool success;
			return GetDictionaryValueSafe(dictionary, key, out success);
		}

		public static TValue GetDictionaryValueSafe<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, out bool success)
		{
			success = false;
			if (dictionary == null)
			{
				return default(TValue);
			}
			if (!dictionary.TryGetValue(key, out var value))
			{
				return default(TValue);
			}
			success = true;
			return value;
		}

		public static bool GetDictionaryValueSafe<TReturn>(Dictionary<string, object> dictionary, string key, ref TReturn value)
		{
			if (dictionary == null)
			{
				return false;
			}
			if (!dictionary.TryGetValue(key, out var value2))
			{
				return false;
			}
			if (value2 == null)
			{
				try
				{
					value = (TReturn)value2;
				}
				catch
				{
					return false;
				}
			}
			if (!(value2 is TReturn))
			{
				return false;
			}
			value = (TReturn)value2;
			return true;
		}

		public static bool GetDictionaryValueSafe(Dictionary<string, object> dictionary, string key, Type type, ref object value)
		{
			if (dictionary == null || type == null)
			{
				return false;
			}
			if (!dictionary.TryGetValue(key, out var value2))
			{
				return false;
			}
			if (value2 == null)
			{
				value = value2;
				return true;
			}
			if (!ReflectionTools.DoesTypeImplement(value2.GetType(), type))
			{
				return false;
			}
			value = value2;
			return true;
		}

		public static bool GetDictionaryValueSafe_float(Dictionary<string, object> dictionary, string key, ref float value)
		{
			if (dictionary == null)
			{
				return false;
			}
			if (!dictionary.TryGetValue(key, out var value2))
			{
				return false;
			}
			if (value2 is float)
			{
				value = (float)value2;
				return true;
			}
			if (value2 is int)
			{
				value = (int)value2;
				return true;
			}
			if (value2 is double)
			{
				value = (float)(double)value2;
				return true;
			}
			return false;
		}

		public static bool GetDictionaryValueSafe_int(Dictionary<string, object> dictionary, string key, ref int value)
		{
			if (dictionary == null)
			{
				return false;
			}
			if (!dictionary.TryGetValue(key, out var value2))
			{
				return false;
			}
			if (value2 is float)
			{
				value = (int)(float)value2;
				return true;
			}
			if (value2 is int)
			{
				value = (int)value2;
				return true;
			}
			if (value2 is double)
			{
				value = (int)(double)value2;
				return true;
			}
			return false;
		}

		public static void AddValueSafe(Dictionary<string, object> data, string key, object value)
		{
			if (data == null || string.IsNullOrEmpty(key))
			{
				return;
			}
			if (value == null)
			{
				if (data.ContainsKey(key))
				{
					data.Remove(key);
				}
			}
			else if (data.ContainsKey(key))
			{
				data[key] = value;
			}
			else
			{
				data.Add(key, value);
			}
		}

		public static T GetValue<T>(IEnumerable<T> enumerable, int index)
		{
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				if (num == index)
				{
					return enumerator.Current;
				}
				num++;
			}
			return default(T);
		}

		public static T GetValue<T>(IEnumerable enumerable, int index)
		{
			IEnumerator enumerator = enumerable.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				if (num == index)
				{
					return (T)enumerator.Current;
				}
				num++;
			}
			return default(T);
		}

		public static void Enqueue<T>(IObjectPool<T> pool, RingBuffer<T> buffer, T item, out bool overrun)
		{
			int count = buffer.Count;
			int capacity = buffer.Capacity;
			if (count == capacity)
			{
				pool.Return(buffer.Dequeue());
				overrun = true;
			}
			else
			{
				overrun = false;
			}
			buffer.Enqueue(item);
		}

		public static void Clear<T>(IObjectPool<T> pool, RingBuffer<T> buffer)
		{
			int count = buffer.Count;
			for (int i = 0; i < count; i++)
			{
				pool.Return(buffer[i]);
			}
			buffer.Clear();
		}
	}
}
