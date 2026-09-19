using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableListFormatter<T> : CollectionFormatterBase<T, ImmutableList<T>.Builder, ImmutableList<T>.Enumerator, ImmutableList<T>>
	{
		protected override void Add(ImmutableList<T>.Builder collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override ImmutableList<T> Complete(ImmutableList<T>.Builder intermediateCollection)
		{
			return intermediateCollection.ToImmutable();
		}

		protected override ImmutableList<T>.Builder Create(int count, MessagePackSerializerOptions options)
		{
			return ImmutableList.CreateBuilder<T>();
		}

		protected override ImmutableList<T>.Enumerator GetSourceEnumerator(ImmutableList<T> source)
		{
			return source.GetEnumerator();
		}
	}
}
