using System;
using Unity.Collections;

namespace Zorro.Core
{
	public class NativeHashedBookkeeper<T> : NativeBookkeeper<T> where T : unmanaged, IEquatable<T>
	{
		public BidirectionalNativeDictionary<T, int> IndexHashMap;

		public NativeHashedBookkeeper(int defaultCapacity)
			: base(defaultCapacity)
		{
			IndexHashMap = new BidirectionalNativeDictionary<T, int>(defaultCapacity, Allocator.Persistent);
		}

		public override int Add(T newEntry)
		{
			int num = base.Add(newEntry);
			IndexHashMap.Add(newEntry, num);
			return num;
		}

		public override BookkeperRemovalInfo Remove(T entry)
		{
			BookkeperRemovalInfo result = base.Remove(entry);
			IndexHashMap.RemoveFromKey(entry);
			if (result.IndexRemoved != result.SwapbackIndex)
			{
				T key = IndexHashMap.RemoveFromValue(result.SwapbackIndex);
				IndexHashMap.Add(key, result.IndexRemoved);
			}
			return result;
		}

		public bool HasKey(T key)
		{
			return IndexHashMap.Contains(key);
		}
	}
}
