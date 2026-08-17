using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameKit.Utilities
{
	public static class ResettableCollectionCaches<T1, T2> where T1 : IResettable where T2 : IResettable
	{
		public static Dictionary<T1, T2> RetrieveDictionary()
		{
			return CollectionCaches<T1, T2>.RetrieveDictionary();
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
			foreach (KeyValuePair<T1, T2> item in value)
			{
				item.Key.ResetState();
				ObjectCaches<T1>.Store(item.Key);
				item.Value.ResetState();
				ObjectCaches<T2>.Store(item.Value);
			}
			value.Clear();
			CollectionCaches<T1, T2>.Store(value);
		}
	}
	public static class ResettableCollectionCaches<T> where T : IResettable
	{
		public static T[] RetrieveArray()
		{
			return CollectionCaches<T>.RetrieveArray();
		}

		public static List<T> RetrieveList()
		{
			return CollectionCaches<T>.RetrieveList();
		}

		public static HashSet<T> RetrieveHashSet()
		{
			return CollectionCaches<T>.RetrieveHashSet();
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
				value[i].ResetState();
				ObjectCaches<T>.Store(value[i]);
			}
			CollectionCaches<T>.Store(value, count);
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
			for (int i = 0; i < value.Count; i++)
			{
				value[i].ResetState();
				ObjectCaches<T>.Store(value[i]);
			}
			value.Clear();
			CollectionCaches<T>.Store(value);
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
			foreach (T item in value)
			{
				item.ResetState();
				ObjectCaches<T>.Store(item);
			}
			value.Clear();
			CollectionCaches<T>.Store(value);
		}
	}
}
