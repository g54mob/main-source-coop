using System;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	public sealed class SingleArrayFormatter : IMessagePackFormatter<float[]?>, IMessagePackFormatter
	{
		public static readonly SingleArrayFormatter Instance = new SingleArrayFormatter();

		private SingleArrayFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, float[]? value, MessagePackSerializerOptions options)
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

		public float[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			int num = reader.ReadArrayHeader();
			if (num == 0)
			{
				return Array.Empty<float>();
			}
			float[] array = new float[num];
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				array[i] = reader.ReadSingle();
			}
			return array;
		}
	}
}
