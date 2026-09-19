using System;
using System.Collections.Generic;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class GenericEnumerableFormatter<TElement, TCollection> : CollectionFormatterBase<TElement, TElement[], TCollection> where TCollection : IEnumerable<TElement>
	{
		protected override TElement[] Create(int count, MessagePackSerializerOptions options)
		{
			return new TElement[count];
		}

		protected override void Add(TElement[] collection, int index, TElement value, MessagePackSerializerOptions options)
		{
			collection[index] = value;
		}

		protected override TCollection Complete(TElement[] intermediateCollection)
		{
			return (TCollection)Activator.CreateInstance(typeof(TCollection), intermediateCollection);
		}
	}
}
