using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, ImmutableDictionary<TKey, TValue>.Builder, ImmutableDictionary<TKey, TValue>.Enumerator, ImmutableDictionary<TKey, TValue>>
	{
		protected override void Add(ImmutableDictionary<TKey, TValue>.Builder collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override ImmutableDictionary<TKey, TValue> Complete(ImmutableDictionary<TKey, TValue>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableDictionary<TKey, TValue>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableDictionary.CreateBuilder<TKey, TValue>(options.Security.GetEqualityComparer<TKey>());
		}

		protected override ImmutableDictionary<TKey, TValue>.Enumerator GetSourceEnumerator(ImmutableDictionary<TKey, TValue> source)
		{
			return source.GetEnumerator();
		}
	}
}
