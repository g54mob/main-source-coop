using System.Numerics;

namespace MessagePack.Formatters
{
	public sealed class Matrix4x4Formatter : IMessagePackFormatter<Matrix4x4>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<Matrix4x4> Instance = new Matrix4x4Formatter();

		private Matrix4x4Formatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, Matrix4x4 value, MessagePackSerializerOptions options)
		{
			writer.WriteArrayHeader(16);
			writer.Write(value.M11);
			writer.Write(value.M12);
			writer.Write(value.M13);
			writer.Write(value.M14);
			writer.Write(value.M21);
			writer.Write(value.M22);
			writer.Write(value.M23);
			writer.Write(value.M24);
			writer.Write(value.M31);
			writer.Write(value.M32);
			writer.Write(value.M33);
			writer.Write(value.M34);
			writer.Write(value.M41);
			writer.Write(value.M42);
			writer.Write(value.M43);
			writer.Write(value.M44);
		}

		public Matrix4x4 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.ReadArrayHeader() != 16)
			{
				throw new MessagePackSerializationException("Invalid Matrix4x4 data.");
			}
			return new Matrix4x4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
