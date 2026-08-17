using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameKit.Utilities
{
	public static class ResettableT1CollectionCaches<T1, T2> where T1 : IResettable
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
			foreach (T1 key in value.Keys)
			{
				key.ResetState();
				ObjectCaches<T1>.Store(key);
			}
			value.Clear();
			CollectionCaches<T1, T2>.Store(value);
		}
	}
}
