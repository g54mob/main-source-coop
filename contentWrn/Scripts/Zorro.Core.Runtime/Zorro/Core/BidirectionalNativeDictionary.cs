using System;
using Unity.Collections;

namespace Zorro.Core
{
	public struct BidirectionalNativeDictionary<T1, T2> : IDisposable where T1 : unmanaged, IEquatable<T1> where T2 : unmanaged, IEquatable<T2>
	{
		private NativeParallelHashMap<T1, T2> t1ToT2Dict;

		private NativeParallelHashMap<T2, T1> t2ToT1Dict;

		public int Count => t1ToT2Dict.Count();

		public BidirectionalNativeDictionary(int capacity, Allocator allocator)
		{
			t1ToT2Dict = new NativeParallelHashMap<T1, T2>(capacity, allocator);
			t2ToT1Dict = new NativeParallelHashMap<T2, T1>(capacity, allocator);
		}

		public void Add(T1 key, T2 value)
		{
			if (t1ToT2Dict.ContainsKey(key))
			{
				RemoveFromKey(key);
			}
			t1ToT2Dict[key] = value;
			t2ToT1Dict[value] = key;
		}

		public T2 GetFromKey(T1 key)
		{
			return t1ToT2Dict[key];
		}

		public T1 GetKeyFromValue(T2 value)
		{
			return t2ToT1Dict[value];
		}

		public T1 Get(T2 key)
		{
			return t2ToT1Dict[key];
		}

		public bool TryGetValue(T1 key, out T2 value)
		{
			return t1ToT2Dict.TryGetValue(key, out value);
		}

		public bool TryGetValue(T2 key, out T1 value)
		{
			return t2ToT1Dict.TryGetValue(key, out value);
		}

		public bool Contains(T1 key)
		{
			return t1ToT2Dict.ContainsKey(key);
		}

		public bool Contains(T2 key)
		{
			return t2ToT1Dict.ContainsKey(key);
		}

		public void RemoveFromKey(T1 key)
		{
			if (Contains(key))
			{
				T2 key2 = t1ToT2Dict[key];
				t1ToT2Dict.Remove(key);
				t2ToT1Dict.Remove(key2);
			}
		}

		public T1 RemoveFromValue(T2 key)
		{
			if (Contains(key))
			{
				T1 val = t2ToT1Dict[key];
				t1ToT2Dict.Remove(val);
				t2ToT1Dict.Remove(key);
				return val;
			}
			return default(T1);
		}

		public void Dispose()
		{
			t1ToT2Dict.Dispose();
			t2ToT1Dict.Dispose();
		}

		public NativeParallelHashMap<T1, T2> GetNativeHashMapT1ToT2()
		{
			return t1ToT2Dict;
		}
	}
}
