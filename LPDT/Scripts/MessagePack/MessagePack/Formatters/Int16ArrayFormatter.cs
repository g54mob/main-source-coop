using System;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	public sealed class Int16ArrayFormatter : IMessagePackFormatter<short[]?>, IMessagePackFormatter
	{
		public static readonly Int16ArrayFormatter Instance = new Int16ArrayFormatter();

		private Int16ArrayFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, short[]? value, MessagePackSerializerOptions options)
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

		public short[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			int num = reader.ReadArrayHeader();
			if (num == 0)
			{
				return Array.Empty<short>();
			}
			short[] array = new short[num];
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				array[i] = reader.ReadInt16();
			}
			return array;
		}
	}
}
