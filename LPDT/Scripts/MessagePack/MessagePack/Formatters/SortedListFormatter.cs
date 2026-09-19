using System.Collections.Generic;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class SortedListFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, SortedList<TKey, TValue>> where TKey : notnull
	{
		protected override void Add(SortedList<TKey, TValue> collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override SortedList<TKey, TValue> Create(int count, MessagePackSerializerOptions options)
		{
			return new SortedList<TKey, TValue>(count);
		}
	}
}
