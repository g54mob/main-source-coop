using System;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	public sealed class DoubleArrayFormatter : IMessagePackFormatter<double[]?>, IMessagePackFormatter
	{
		public static readonly DoubleArrayFormatter Instance = new DoubleArrayFormatter();

		private DoubleArrayFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, double[]? value, MessagePackSerializerOptions options)
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

		public double[]? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			int num = reader.ReadArrayHeader();
			if (num == 0)
			{
				return Array.Empty<double>();
			}
			double[] array = new double[num];
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				array[i] = reader.ReadDouble();
			}
			return array;
		}
	}
}
