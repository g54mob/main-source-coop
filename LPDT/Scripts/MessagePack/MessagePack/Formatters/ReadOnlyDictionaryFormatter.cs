using System.Collections.Generic;
using System.Collections.ObjectModel;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class ReadOnlyDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, ReadOnlyDictionary<TKey, TValue>> where TKey : notnull
	{
		protected override void Add(Dictionary<TKey, TValue> collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override ReadOnlyDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
		{
			return new ReadOnlyDictionary<TKey, TValue>(intermediateCollection);
		}

		protected override Dictionary<TKey, TValue> Create(int count, MessagePackSerializerOptions options)
		{
			return new Dictionary<TKey, TValue>(count, options.Security.GetEqualityComparer<TKey>());
		}
	}
}
