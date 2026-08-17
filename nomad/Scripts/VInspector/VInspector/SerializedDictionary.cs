using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VInspector
{
	[Serializable]
	public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[Serializable]
		public class SerializedKeyValuePair<TKey_, TValue_>
		{
			public TKey_ Key;

			public TValue_ Value;

			public int index;

			public bool isKeyRepeated;

			public bool isKeyNull;

			public SerializedKeyValuePair(TKey_ key, TValue_ value)
			{
				Key = key;
				Value = value;
			}

			public static implicit operator SerializedKeyValuePair<TKey_, TValue_>(KeyValuePair<TKey_, TValue_> kvp)
			{
				return new SerializedKeyValuePair<TKey_, TValue_>(kvp.Key, kvp.Value);
			}

			public static implicit operator KeyValuePair<TKey_, TValue_>(SerializedKeyValuePair<TKey_, TValue_> kvp)
			{
				return new KeyValuePair<TKey_, TValue_>(kvp.Key, kvp.Value);
			}
		}

		public List<SerializedKeyValuePair<TKey, TValue>> serializedKvps = new List<SerializedKeyValuePair<TKey, TValue>>();

		public float dividerPos = 0.33f;

		public void OnBeforeSerialize()
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<TKey, TValue> kvp = enumerator.Current;
					SerializedKeyValuePair<TKey, TValue> serializedKeyValuePair = serializedKvps.FirstOrDefault((SerializedKeyValuePair<TKey, TValue> r) => base.Comparer.Equals(r.Key, kvp.Key));
					if (serializedKeyValuePair != null)
					{
						serializedKeyValuePair.Value = kvp.Value;
					}
					else
					{
						serializedKvps.Add(kvp);
					}
				}
			}
			serializedKvps.RemoveAll((SerializedKeyValuePair<TKey, TValue> r) => r.Key != null && !ContainsKey(r.Key));
			for (int num = 0; num < serializedKvps.Count; num++)
			{
				serializedKvps[num].index = num;
			}
		}

		public void OnAfterDeserialize()
		{
			Clear();
			foreach (SerializedKeyValuePair<TKey, TValue> serializedKvp in serializedKvps)
			{
				serializedKvp.isKeyNull = serializedKvp.Key == null;
				serializedKvp.isKeyRepeated = serializedKvp.Key != null && ContainsKey(serializedKvp.Key);
				if (!serializedKvp.isKeyNull && !serializedKvp.isKeyRepeated)
				{
					Add(serializedKvp.Key, serializedKvp.Value);
				}
			}
		}
	}
}
