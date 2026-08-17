using System;
using System.Collections;
using System.Collections.Generic;

namespace Den.Tools
{
	public class DictionaryOrdered<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IList<TKey>, ICollection<TKey>, IEnumerable<TKey>, ICollection, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyCollection<TKey>
	{
		protected Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

		protected List<TKey> order = new List<TKey>();

		public Dictionary<TKey, TValue>.ValueCollection Values => dict.Values;

		public Dictionary<TKey, TValue>.KeyCollection Keys => dict.Keys;

		ICollection<TValue> IDictionary<TKey, TValue>.Values => dict.Values;

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => dict.Keys;

		public int Count => order.Count;

		public IEqualityComparer<TKey> Comparer => dict.Comparer;

		public TValue this[TKey key]
		{
			get
			{
				return dict[key];
			}
			set
			{
				dict[key] = value;
			}
		}

		public TValue this[int num]
		{
			get
			{
				return dict[order[num]];
			}
			set
			{
				dict[order[num]] = value;
			}
		}

		TKey IList<TKey>.this[int num]
		{
			get
			{
				return order[num];
			}
			set
			{
				order[num] = value;
			}
		}

		bool ICollection.IsSynchronized => ((ICollection)dict).IsSynchronized;

		object ICollection.SyncRoot => ((ICollection)dict).SyncRoot;

		public bool IsReadOnly
		{
			get
			{
				if (((ICollection<KeyValuePair<TKey, TValue>>)dict).IsReadOnly)
				{
					return ((ICollection<TKey>)order).IsReadOnly;
				}
				return false;
			}
		}

		public DictionaryOrdered()
		{
		}

		public DictionaryOrdered(IEqualityComparer<TKey> comparer)
		{
			dict = new Dictionary<TKey, TValue>(comparer);
			order = new List<TKey>();
		}

		public DictionaryOrdered(int capacity)
		{
			dict = new Dictionary<TKey, TValue>(capacity);
			order = new List<TKey>(capacity);
		}

		public DictionaryOrdered(int capacity, IEqualityComparer<TKey> comparer)
		{
			dict = new Dictionary<TKey, TValue>(capacity, comparer);
			order = new List<TKey>(capacity);
		}

		public DictionaryOrdered(DictionaryOrdered<TKey, TValue> other)
		{
			dict = new Dictionary<TKey, TValue>(other.dict);
			order = new List<TKey>(other.order);
		}

		public TKey GetKeyByNum(int num)
		{
			return order[num];
		}

		public void SetKeyByNum(int num, TKey val)
		{
			order[num] = val;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return dict.TryGetValue(key, out value);
		}

		public void Add(TKey key, TValue value)
		{
			dict.Add(key, value);
			order.Add(key);
		}

		public void Add(KeyValuePair<TKey, TValue> kvp)
		{
			dict.Add(kvp.Key, kvp.Value);
			order.Add(kvp.Key);
		}

		public void Add(TKey key)
		{
			dict.Add(key, default(TValue));
			order.Add(key);
		}

		public bool TryAdd(TKey key, TValue value)
		{
			if (dict.ContainsKey(key))
			{
				return false;
			}
			dict.Add(key, value);
			order.Add(key);
			return true;
		}

		public void Insert(int index, TKey key, TValue value)
		{
			dict.Add(key, value);
			order.Insert(index, key);
		}

		public void Insert(int index, TKey key)
		{
			dict.Add(key, default(TValue));
			order.Insert(index, key);
		}

		public int IndexOf(TKey key)
		{
			return order.IndexOf(key);
		}

		public void Clear()
		{
			dict.Clear();
			order.Clear();
		}

		public bool Remove(TKey key)
		{
			if (!dict.ContainsKey(key))
			{
				return false;
			}
			int num = order.IndexOf(key);
			if (num < 0)
			{
				return false;
			}
			dict.Remove(key);
			order.RemoveAt(num);
			return true;
		}

		public bool Remove(KeyValuePair<TKey, TValue> kvp)
		{
			if (!((ICollection<KeyValuePair<TKey, TValue>>)dict).Contains(kvp))
			{
				return false;
			}
			int num = order.IndexOf(kvp.Key);
			if (num < 0)
			{
				return false;
			}
			dict.Remove(kvp.Key);
			order.RemoveAt(num);
			return true;
		}

		public void RemoveAt(int index)
		{
			dict.Remove(order[index]);
			order.RemoveAt(index);
		}

		public void Switch(int n1, int n2)
		{
			TKey value = order[n1];
			order[n1] = order[n2];
			order[n2] = value;
		}

		public bool ContainsKey(TKey key)
		{
			return dict.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			return dict.ContainsValue(value);
		}

		public bool Contains(TKey key)
		{
			return dict.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<TKey, TValue> kvp)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)dict).Contains(kvp);
		}

		public void CopyTo(Array array, int index)
		{
			int num = ((array.Length < order.Count + index) ? array.Length : (order.Count + index));
			for (int i = index; i < num; i++)
			{
				TKey key = order[i - index];
				array.SetValue(new KeyValuePair<TKey, TValue>(key, dict[key]), i);
			}
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			CopyTo((Array)array, index);
		}

		public void CopyTo(TKey[] array, int index)
		{
			order.CopyTo(array, index);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (TKey item in order)
			{
				yield return new KeyValuePair<TKey, TValue>(item, dict[item]);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (TKey item in order)
			{
				yield return new KeyValuePair<TKey, TValue>(item, dict[item]);
			}
		}

		IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
		{
			return order.GetEnumerator();
		}

		public DictionaryOrdered(TKey[] keys, TValue[] values)
		{
			if (keys.Length != values.Length)
			{
				throw new Exception("Could not create ordered dictionary: keys and values lengths differ");
			}
			dict = new Dictionary<TKey, TValue>(keys.Length);
			for (int i = 0; i < keys.Length; i++)
			{
				dict.Add(keys[i], values[i]);
			}
			order = new List<TKey>(keys.Length);
			order.AddRange(keys);
		}

		public DictionaryOrdered(TKey[] keys)
		{
			dict = new Dictionary<TKey, TValue>(keys.Length);
			order = new List<TKey>(keys.Length);
			dict = new Dictionary<TKey, TValue>(keys.Length);
			for (int i = 0; i < keys.Length; i++)
			{
				dict.Add(keys[i], default(TValue));
			}
			order = new List<TKey>(keys.Length);
			order.AddRange(keys);
		}

		public void TakeMatchingValuesFrom(DictionaryOrdered<TKey, TValue> src)
		{
			foreach (TKey item in order)
			{
				if (src.dict.TryGetValue(item, out var value))
				{
					dict[item] = value;
				}
			}
		}

		public (TKey[], TValue[]) Serialize()
		{
			int count = Count;
			TKey[] array = new TKey[Count];
			TValue[] array2 = new TValue[Count];
			for (int i = 0; i < count; i++)
			{
				TKey key = (array[i] = order[i]);
				array2[i] = dict[key];
			}
			return (array, array2);
		}

		public void Serialize(ref TKey[] keys, ref TValue[] vals)
		{
			int count = Count;
			if (keys.Length != count)
			{
				keys = new TKey[Count];
			}
			if (vals.Length != count)
			{
				vals = new TValue[Count];
			}
			for (int i = 0; i < count; i++)
			{
				TKey val = order[i];
				keys[i] = val;
				vals[i] = dict[val];
			}
		}

		public void Deserialize(TKey[] keys, TValue[] values)
		{
			if (keys.Length != values.Length)
			{
				throw new Exception("Could not deserialize ordered dictionary: keys and values lengths differ");
			}
			Clear();
			for (int i = 0; i < keys.Length; i++)
			{
				Add(keys[i], values[i]);
			}
		}

		public void ReCreateDictionary()
		{
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> item in dict)
			{
				dictionary.Add(item.Key, item.Value);
			}
			dict = dictionary;
		}
	}
}
