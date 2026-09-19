using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	public sealed class BooleanArrayFormatter : IMessagePackFormatter<bool[]?>, IMessagePackFormatter
	{
		public static readonly BooleanArrayFormatter Instance = new BooleanArrayFormatter();

		private BooleanArrayFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, bool[]? value, MessagePackSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNil();
				return;
			}
			writer.WriteArrayHeader(value.Length);
			if (value.Length != 0)
			{
				writer.CancellationToken.ThrowIfCancellationRequested();
				UnsafeRefSerializeHelper.Serialize(ref writer, ref value[0], value.Length);
			}
		}

		public bool[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			int num = reader.ReadArrayHeader();
			if (num == 0)
			{
				return Array.Empty<bool>();
			}
			bool[] array = new bool[num];
			ref bool source = ref array[0];
			int num2 = 0;
			ReadOnlySequence<byte>.Enumerator enumerator = reader.ReadRaw(num).GetEnumerator();
			checked
			{
				while (enumerator.MoveNext())
				{
					ReadOnlySpan<byte> span = enumerator.Current.Span;
					if (!span.IsEmpty)
					{
						int num3 = UnsafeRefDeserializeHelper.Deserialize(ref Unsafe.AsRef(in span[0]), span.Length, ref Unsafe.Add(ref source, num2));
						if (num3 >= 0)
						{
							throw new MessagePackSerializationException($"Unexpected msgpack code {span[num3]} ({MessagePackCode.ToFormatName(span[num3])}) at {num3 + num2} encountered.");
						}
						num2 += span.Length;
					}
				}
				return array;
			}
		}
	}
}
