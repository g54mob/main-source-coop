using System;
using System.Buffers;

namespace MessagePack.Formatters
{
	public sealed class ByteArrayFormatter : IMessagePackFormatter<byte[]?>, IMessagePackFormatter
	{
		public static readonly ByteArrayFormatter Instance = new ByteArrayFormatter();

		private ByteArrayFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, byte[]? value, MessagePackSerializerOptions options)
		{
			writer.Write(value);
		}

		public byte[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			checked
			{
				if (reader.NextMessagePackType == MessagePackType.Array)
				{
					int num = reader.ReadArrayHeader();
					if (num == 0)
					{
						return Array.Empty<byte>();
					}
					byte[] array = new byte[num];
					options.Security.DepthStep(ref reader);
					try
					{
						for (int i = 0; i < num; i++)
						{
							reader.CancellationToken.ThrowIfCancellationRequested();
							array[i] = reader.ReadByte();
						}
						return array;
					}
					finally
					{
						reader.Depth--;
					}
				}
				ReadOnlySequence<byte>? readOnlySequence = reader.ReadBytes();
				if (!readOnlySequence.HasValue)
				{
					return null;
				}
				return readOnlySequence.GetValueOrDefault().ToArray<byte>();
			}
		}
	}
}
