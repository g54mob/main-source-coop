using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableSortedSetFormatter<T> : CollectionFormatterBase<T, ImmutableSortedSet<T>.Builder, ImmutableSortedSet<T>.Enumerator, ImmutableSortedSet<T>>
	{
		protected override void Add(ImmutableSortedSet<T>.Builder collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override ImmutableSortedSet<T> Complete(ImmutableSortedSet<T>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableSortedSet<T>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableSortedSet.CreateBuilder<T>();
		}

		protected override ImmutableSortedSet<T>.Enumerator GetSourceEnumerator(ImmutableSortedSet<T> source)
		{
			return source.GetEnumerator();
		}
	}
}
