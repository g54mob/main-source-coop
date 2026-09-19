using System.Numerics;

namespace MessagePack.Formatters
{
	public sealed class QuaternionFormatter : IMessagePackFormatter<Quaternion>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<Quaternion> Instance = new QuaternionFormatter();

		private QuaternionFormatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, Quaternion value, MessagePackSerializerOptions options)
		{
			writer.WriteArrayHeader(4);
			writer.Write(value.X);
			writer.Write(value.Y);
			writer.Write(value.Z);
			writer.Write(value.W);
		}

		public Quaternion Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.ReadArrayHeader() != 4)
			{
				throw new MessagePackSerializationException("Invalid Quaternion data.");
			}
			return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
