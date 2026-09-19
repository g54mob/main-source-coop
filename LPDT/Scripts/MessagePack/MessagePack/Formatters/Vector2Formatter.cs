using System.Numerics;

namespace MessagePack.Formatters
{
	public sealed class Vector2Formatter : IMessagePackFormatter<Vector2>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<Vector2> Instance = new Vector2Formatter();

		private Vector2Formatter()
		{
		}

		public void Serialize(ref MessagePackWriter writer, Vector2 value, MessagePackSerializerOptions options)
		{
			writer.WriteArrayHeader(2);
			writer.Write(value.X);
			writer.Write(value.Y);
		}

		public Vector2 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.ReadArrayHeader() != 2)
			{
				throw new MessagePackSerializationException("Invalid Vector2 data.");
			}
			return new Vector2(reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
