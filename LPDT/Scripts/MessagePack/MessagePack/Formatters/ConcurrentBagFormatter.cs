using System.Collections.Concurrent;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class ConcurrentBagFormatter<T> : CollectionFormatterBase<T, ConcurrentBag<T>>
	{
		protected override int? GetCount(ConcurrentBag<T> sequence)
		{
			return sequence.Count;
		}

		protected override void Add(ConcurrentBag<T> collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override ConcurrentBag<T> Create(int count, MessagePackSerializerOptions options)
		{
			return new ConcurrentBag<T>();
		}
	}
}
