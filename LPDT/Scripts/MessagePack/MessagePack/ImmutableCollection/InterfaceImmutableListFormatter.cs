using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class InterfaceImmutableListFormatter<T> : CollectionFormatterBase<T, ImmutableList<T>.Builder, IImmutableList<T>>
	{
		protected override void Add(ImmutableList<T>.Builder collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override IImmutableList<T> Complete(ImmutableList<T>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableList<T>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableList.CreateBuilder<T>();
		}
	}
}
