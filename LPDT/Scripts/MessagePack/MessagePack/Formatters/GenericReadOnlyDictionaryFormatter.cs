using System;
using System.Collections.Generic;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class GenericReadOnlyDictionaryFormatter<TKey, TValue, TDictionary> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, TDictionary> where TKey : notnull where TDictionary : class?, IReadOnlyDictionary<TKey, TValue>
	{
		protected override void Add(Dictionary<TKey, TValue> collection, int index, TKey key, TValue value, MessagePackSerializerOptions options)
		{
			collection.Add(key, value);
		}

		protected override Dictionary<TKey, TValue> Create(int count, MessagePackSerializerOptions options)
		{
			return new Dictionary<TKey, TValue>(count, options.Security.GetEqualityComparer<TKey>());
		}

		protected override TDictionary Complete(Dictionary<TKey, TValue> intermediateCollection)
		{
			return (TDictionary)(Activator.CreateInstance(typeof(TDictionary), intermediateCollection) ?? throw new InvalidOperationException("Unable to create dictionary instance."));
		}
	}
}
