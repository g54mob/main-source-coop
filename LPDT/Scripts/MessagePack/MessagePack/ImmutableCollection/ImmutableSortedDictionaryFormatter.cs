using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableSortedDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, ImmutableSortedDictionary<TKey, TValue>.Builder, ImmutableSortedDictionary<TKey, TValue>.Enumerator, ImmutableSortedDictionary<TKey, TValue>>
	{
		protected override void Add(ImmutableSortedDictionary<TKey, TValue>.Builder collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override ImmutableSortedDictionary<TKey, TValue> Complete(ImmutableSortedDictionary<TKey, TValue>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableSortedDictionary<TKey, TValue>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableSortedDictionary.CreateBuilder<TKey, TValue>();
		}

		protected override ImmutableSortedDictionary<TKey, TValue>.Enumerator GetSourceEnumerator(ImmutableSortedDictionary<TKey, TValue> source)
		{
			return source.GetEnumerator();
		}
	}
}
