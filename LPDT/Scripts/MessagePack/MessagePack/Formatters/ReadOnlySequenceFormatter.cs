using System;
using System.Buffers;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class ReadOnlySequenceFormatter<T> : IMessagePackFormatter<ReadOnlySequence<T>>, IMessagePackFormatter
	{
		public void Serialize(ref MessagePackWriter writer, ReadOnlySequence<T> value, MessagePackSerializerOptions options)
		{
			IMessagePackFormatter<T> formatterWithVerify = options.Resolver.GetFormatterWithVerify<T>();
			checked
			{
				writer.WriteArrayHeader((int)value.Length);
				ReadOnlySequence<T>.Enumerator enumerator = value.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ReadOnlySpan<T> span = enumerator.Current.Span;
					for (int i = 0; i < span.Length; i++)
					{
						writer.CancellationToken.ThrowIfCancellationRequested();
						formatterWithVerify.Serialize(ref writer, span[i], options);
					}
				}
			}
		}

		public ReadOnlySequence<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			return new ReadOnlySequence<T>(options.Resolver.GetFormatterWithVerify<T[]>().Deserialize(ref reader, options));
		}
	}
}
