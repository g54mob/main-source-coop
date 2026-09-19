using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableHashSetFormatter<T> : CollectionFormatterBase<T, ImmutableHashSet<T>.Builder, ImmutableHashSet<T>.Enumerator, ImmutableHashSet<T>>
	{
		protected override void Add(ImmutableHashSet<T>.Builder collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override ImmutableHashSet<T> Complete(ImmutableHashSet<T>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableHashSet<T>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableHashSet.CreateBuilder(options.Security.GetEqualityComparer<T>());
		}

		protected override ImmutableHashSet<T>.Enumerator GetSourceEnumerator(ImmutableHashSet<T> source)
		{
			return source.GetEnumerator();
		}
	}
}
