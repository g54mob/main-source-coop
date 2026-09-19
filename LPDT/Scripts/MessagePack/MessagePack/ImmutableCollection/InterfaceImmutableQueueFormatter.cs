using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class InterfaceImmutableQueueFormatter<T> : CollectionFormatterBase<T, ImmutableQueueBuilder<T>, IImmutableQueue<T>>
	{
		protected override void Add(ImmutableQueueBuilder<T> collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override IImmutableQueue<T> Complete(ImmutableQueueBuilder<T> intermediateCollection)
		{
			return intermediateCollection.Q;
		}

		protected override ImmutableQueueBuilder<T> Create(int count, MessagePackSerializerOptions options)
		{
			return new ImmutableQueueBuilder<T>();
		}
	}
}
