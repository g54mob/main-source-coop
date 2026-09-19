using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class InterfaceImmutableDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, ImmutableDictionary<TKey, TValue>.Builder, IImmutableDictionary<TKey, TValue>>
	{
		protected override void Add(ImmutableDictionary<TKey, TValue>.Builder collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override IImmutableDictionary<TKey, TValue> Complete(ImmutableDictionary<TKey, TValue>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableDictionary<TKey, TValue>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableDictionary.CreateBuilder<TKey, TValue>(options.Security.GetEqualityComparer<TKey>());
		}
	}
}
