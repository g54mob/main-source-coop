using System.Collections.Generic;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class GenericDictionaryFormatter<TKey, TValue, TDictionary> : DictionaryFormatterBase<TKey, TValue, TDictionary> where TKey : notnull where TDictionary : class?, IDictionary<TKey, TValue>, new()
	{
		protected override void Add(TDictionary collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override TDictionary Create(int count, MessagePackSerializerOptions options)
		{
			return CollectionHelpers<TDictionary, IEqualityComparer<TKey>>.CreateHashCollection(count, options.Security.GetEqualityComparer<TKey>());
		}
	}
}
