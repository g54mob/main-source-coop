using System;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	public sealed class Int32ArrayFormatter : IMessagePackFormatter<int[]?>, IMessagePackFormatter
	{
		public static readonly Int32ArrayFormatter Instance = new Int32ArrayFormatter();

		private Int32ArrayFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, int[]? value, MessagePackSerializerOptions options)
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

		public int[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			int num = reader.ReadArrayHeader();
			if (num == 0)
			{
				return Array.Empty<int>();
			}
			int[] array = new int[num];
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				array[i] = reader.ReadInt32();
			}
			return array;
		}
	}
}
