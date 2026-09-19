using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global.SerializableDictionary
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		[SerializeField]
		private List<SerializableKeyValuePair<TKey, TValue>> items = new List<SerializableKeyValuePair<TKey, TValue>>();

		public int Count => items.Count;

		public bool IsReadOnly => false;

		public ICollection<TKey> Keys => items.ConvertAll((SerializableKeyValuePair<TKey, TValue> item) => item.key);

		public ICollection<TValue> Values => items.ConvertAll((SerializableKeyValuePair<TKey, TValue> item) => item.value);

		public TValue this[TKey key]
		{
			get
			{
				return items.Find(delegate(SerializableKeyValuePair<TKey, TValue> item)
				{
					ref TKey key2 = ref item.key;
					object obj = key;
					return key2.Equals(obj);
				}).value;
			}
			set
			{
				for (int i = 0; i < items.Count; i++)
				{
					SerializableKeyValuePair<TKey, TValue> serializableKeyValuePair = items[i];
					if (serializableKeyValuePair.key.Equals(key))
					{
						SerializableKeyValuePair<TKey, TValue> value2 = items[i];
						value2.value = value;
						items[i] = value2;
						return;
					}
				}
				items.Add(new SerializableKeyValuePair<TKey, TValue>
				{
					key = key,
					value = value
				});
			}
		}

		public void OnBeforeSerialize()
		{
			for (int i = 0; i < items.Count; i++)
			{
				SerializableKeyValuePair<TKey, TValue> value = items[i];
				value.isDuplicate = false;
				items[i] = value;
			}
			for (int j = 0; j < items.Count; j++)
			{
				for (int k = j + 1; k < items.Count; k++)
				{
					SerializableKeyValuePair<TKey, TValue> serializableKeyValuePair = items[j];
					ref TKey key = ref serializableKeyValuePair.key;
					object obj = items[k].key;
					if (key.Equals(obj))
					{
						SerializableKeyValuePair<TKey, TValue> value2 = items[j];
						value2.isDuplicate = true;
						items[j] = value2;
						value2 = items[k];
						value2.isDuplicate = true;
						items[k] = value2;
					}
				}
			}
		}

		public void OnAfterDeserialize()
		{
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return items.ConvertAll((SerializableKeyValuePair<TKey, TValue> item) => new KeyValuePair<TKey, TValue>(item.key, item.value)).GetEnumerator();
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			items.Add(new SerializableKeyValuePair<TKey, TValue>
			{
				key = item.Key,
				value = item.Value
			});
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return items.Contains(new SerializableKeyValuePair<TKey, TValue>
			{
				key = item.Key,
				value = item.Value
			});
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			items.ForEach(delegate(SerializableKeyValuePair<TKey, TValue> item)
			{
				array[arrayIndex] = new KeyValuePair<TKey, TValue>(item.key, item.value);
				arrayIndex++;
			});
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return items.Remove(new SerializableKeyValuePair<TKey, TValue>
			{
				key = item.Key,
				value = item.Value
			});
		}

		public void Add(TKey key, TValue value)
		{
			items.Add(new SerializableKeyValuePair<TKey, TValue>
			{
				key = key,
				value = value
			});
		}

		public bool ContainsKey(TKey key)
		{
			return items.Exists(delegate(SerializableKeyValuePair<TKey, TValue> item)
			{
				ref TKey key2 = ref item.key;
				object obj = key;
				return key2.Equals(obj);
			});
		}

		public bool Remove(TKey key)
		{
			for (int i = 0; i < items.Count; i++)
			{
				SerializableKeyValuePair<TKey, TValue> serializableKeyValuePair = items[i];
				if (serializableKeyValuePair.key.Equals(key))
				{
					items.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			value = default(TValue);
			foreach (SerializableKeyValuePair<TKey, TValue> item in items)
			{
				TKey key2 = item.key;
				if (key2.Equals(key))
				{
					value = item.value;
					return true;
				}
			}
			return false;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
