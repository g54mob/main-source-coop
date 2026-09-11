using System.Collections;
using System.Collections.Generic;

namespace Zorro.Core
{
	public class BidirectionalDictionary<T1, T2> : IEnumerable
	{
		private Dictionary<T1, T2> t1ToT2Dict = new Dictionary<T1, T2>();

		private Dictionary<T2, T1> t2ToT1Dict = new Dictionary<T2, T1>();

		public IEnumerable<T1> FirstTypes => t1ToT2Dict.Keys;

		public IEnumerable<T2> SecondTypes => t2ToT1Dict.Keys;

		public int Count => t1ToT2Dict.Count;

		public IEnumerator GetEnumerator()
		{
			return t1ToT2Dict.GetEnumerator();
		}

		public BidirectionalDictionary(int capacity)
		{
			t1ToT2Dict = new Dictionary<T1, T2>(capacity);
			t2ToT1Dict = new Dictionary<T2, T1>(capacity);
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

		public bool ContainsKey(T1 key)
		{
			return t1ToT2Dict.ContainsKey(key);
		}

		public bool Contains(T2 key)
		{
			return t2ToT1Dict.ContainsKey(key);
		}

		public void RemoveFromKey(T1 key)
		{
			if (ContainsKey(key))
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

		public IEnumerable<T2> GetValues()
		{
			return t1ToT2Dict.Values;
		}

		public IEnumerable<T1> GetKeys()
		{
			return t2ToT1Dict.Values;
		}

		public void Clear()
		{
			t1ToT2Dict.Clear();
			t2ToT1Dict.Clear();
		}
	}
}
