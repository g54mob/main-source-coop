using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		public List<TKey> keys = new List<TKey>();

		public List<TValue> values = new List<TValue>();

		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<TKey, TValue> current = enumerator.Current;
				keys.Add(current.Key);
				values.Add(current.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			Clear();
			for (int i = 0; i < keys.Count; i++)
			{
				base[keys[i]] = values[i];
			}
		}
	}
}
