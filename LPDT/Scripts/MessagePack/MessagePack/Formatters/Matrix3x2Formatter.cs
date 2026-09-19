using System.Numerics;

namespace MessagePack.Formatters
{
	public sealed class Matrix3x2Formatter : IMessagePackFormatter<Matrix3x2>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<Matrix3x2> Instance = new Matrix3x2Formatter();

		private Matrix3x2Formatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, Matrix3x2 value, MessagePackSerializerOptions options)
		{
			writer.WriteArrayHeader(6);
			writer.Write(value.M11);
			writer.Write(value.M12);
			writer.Write(value.M21);
			writer.Write(value.M22);
			writer.Write(value.M31);
			writer.Write(value.M32);
		}

		public Matrix3x2 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.ReadArrayHeader() != 6)
			{
				throw new MessagePackSerializationException("Invalid Matrix3x2 data.");
			}
			return new Matrix3x2(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
