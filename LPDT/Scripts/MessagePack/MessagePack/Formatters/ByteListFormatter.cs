using System.Buffers;
using System.Collections.Generic;

namespace MessagePack.Formatters
{
	public sealed class ByteListFormatter : IMessagePackFormatter<List<byte>?>, IMessagePackFormatter
	{
		public static readonly ByteListFormatter Instance = new ByteListFormatter();

		private static readonly ListFormatter<byte> InnerFormatter = new ListFormatter<byte>();

		public void Serialize(ref MessagePackWriter writer, List<byte>? value, MessagePackSerializerOptions options)
		{
			InnerFormatter.Serialize(ref writer, value, options);
		}

		public List<byte>? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.NextMessagePackType == MessagePackType.Array)
			{
				return InnerFormatter.Deserialize(ref reader, options);
			}
			ReadOnlySequence<byte>? readOnlySequence = reader.ReadBytes();
			if (!readOnlySequence.HasValue)
			{
				return null;
			}
			return new List<byte>(readOnlySequence.Value.ToArray<byte>());
		}
	}
}
