using System.Numerics;

namespace MessagePack.Formatters
{
	public sealed class Vector4Formatter : IMessagePackFormatter<Vector4>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<Vector4> Instance = new Vector4Formatter();

		private Vector4Formatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, Vector4 value, MessagePackSerializerOptions options)
		{
			writer.WriteArrayHeader(4);
			writer.Write(value.X);
			writer.Write(value.Y);
			writer.Write(value.Z);
			writer.Write(value.W);
		}

		public Vector4 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.ReadArrayHeader() != 4)
			{
				throw new MessagePackSerializationException("Invalid Vector4 data.");
			}
			return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
