using System;
using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class InterfaceImmutableStackFormatter<T> : CollectionFormatterBase<T, T[], IImmutableStack<T>>
	{
		protected override void Add(T[] collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection[checked(collection.Length - 1 - index)] = value;
		}

		protected override IImmutableStack<T> Complete(T[] intermediateCollection)
		{
			return ImmutableStack.CreateRange(intermediateCollection);
		}

		protected override T[] Create(int count, MessagePackSerializerOptions options)
		{
			if (count != 0)
			{
				return new T[count];
			}
			return Array.Empty<T>();
		}
	}
}
