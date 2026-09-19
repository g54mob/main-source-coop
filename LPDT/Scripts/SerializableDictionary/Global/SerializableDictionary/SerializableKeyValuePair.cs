using System;

namespace Global.SerializableDictionary
{
	[Serializable]
	public struct SerializableKeyValuePair<TKey, TValue>
	{
		public TKey key;

		public TValue value;

		public bool isDuplicate;
	}
}
