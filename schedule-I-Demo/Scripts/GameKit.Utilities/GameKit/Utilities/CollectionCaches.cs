using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameKit.Utilities
{
	public static class CollectionCaches<T1, T2>
	{
		private static readonly Stack<Dictionary<T1, T2>> _dictionaryCache = new Stack<Dictionary<T1, T2>>();

		public static Dictionary<T1, T2> RetrieveDictionary()
		{
			if (_dictionaryCache.Count == 0)
			{
				return new Dictionary<T1, T2>();
			}
			return _dictionaryCache.Pop();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StoreAndDefault(ref Dictionary<T1, T2> value)
		{
			if (value != null)
			{
				Store(value);
				value = null;
			}
		}

		public static void Store(Dictionary<T1, T2> value)
		{
			value.Clear();
			_dictionaryCache.Push(value);
		}
	}
	public static class CollectionCaches<T>
	{
		private static readonly Stack<T[]> _arrayCache = new Stack<T[]>();

		private static readonly Stack<List<T>> _listCache = new Stack<List<T>>();

		private static readonly Stack<HashSet<T>> _hashsetCache = new Stack<HashSet<T>>();

		public static T[] RetrieveArray()
		{
			if (_arrayCache.Count == 0)
			{
				return new T[0];
			}
			return _arrayCache.Pop();
		}

		public static List<T> RetrieveList()
		{
			if (_listCache.Count == 0)
			{
				return new List<T>();
			}
			return _listCache.Pop();
		}

		public static List<T> RetrieveList(T entry)
		{
			List<T> list = ((_listCache.Count != 0) ? _listCache.Pop() : new List<T>());
			list.Add(entry);
			return list;
		}

		public static HashSet<T> RetrieveHashSet()
		{
			if (_hashsetCache.Count == 0)
			{
				return new HashSet<T>();
			}
			return _hashsetCache.Pop();
		}

		public static HashSet<T> RetrieveHashSet(T entry)
		{
			HashSet<T> hashSet = ((_hashsetCache.Count != 0) ? _hashsetCache.Pop() : new HashSet<T>());
			hashSet.Add(entry);
			return hashSet;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StoreAndDefault(ref T[] value, int count)
		{
			if (value != null)
			{
				Store(value, count);
				value = null;
			}
		}

		public static void Store(T[] value, int count)
		{
			for (int i = 0; i < count; i++)
			{
				value[i] = default(T);
			}
			_arrayCache.Push(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StoreAndDefault(ref List<T> value)
		{
			if (value != null)
			{
				Store(value);
				value = null;
			}
		}

		public static void Store(List<T> value)
		{
			value.Clear();
			_listCache.Push(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StoreAndDefault(ref HashSet<T> value)
		{
			if (value != null)
			{
				Store(value);
				value = null;
			}
		}

		public static void Store(HashSet<T> value)
		{
			value.Clear();
			_hashsetCache.Push(value);
		}
	}
}
