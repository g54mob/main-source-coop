using System.Numerics;

namespace MessagePack.Formatters
{
	public sealed class Vector3Formatter : IMessagePackFormatter<Vector3>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<Vector3> Instance = new Vector3Formatter();

		private Vector3Formatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, Vector3 value, MessagePackSerializerOptions options)
		{
			writer.WriteArrayHeader(3);
			writer.Write(value.X);
			writer.Write(value.Y);
			writer.Write(value.Z);
		}

		public Vector3 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.ReadArrayHeader() != 3)
			{
				throw new MessagePackSerializationException("Invalid Vector3 data.");
			}
			return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
