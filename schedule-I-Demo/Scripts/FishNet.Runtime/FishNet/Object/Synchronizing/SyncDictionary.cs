using System.Collections.Generic;
using FishNet.Documenting;

namespace FishNet.Object.Synchronizing
{
	[APIExclude]
	public class SyncDictionary<TKey, TValue> : SyncIDictionary<TKey, TValue>
	{
		[APIExclude]
		public new Dictionary<TKey, TValue>.ValueCollection Values => ((Dictionary<TKey, TValue>)Collection).Values;

		[APIExclude]
		public new Dictionary<TKey, TValue>.KeyCollection Keys => ((Dictionary<TKey, TValue>)Collection).Keys;

		[APIExclude]
		public SyncDictionary()
			: base((IDictionary<TKey, TValue>)new Dictionary<TKey, TValue>())
		{
		}

		[APIExclude]
		public SyncDictionary(IEqualityComparer<TKey> eq)
			: base((IDictionary<TKey, TValue>)new Dictionary<TKey, TValue>(eq))
		{
		}

		[APIExclude]
		public new Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return ((Dictionary<TKey, TValue>)Collection).GetEnumerator();
		}
	}
}
