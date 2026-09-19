namespace MessagePack.Formatters
{
	public sealed class ForceTypelessFormatter<T> : IMessagePackFormatter<T?>, IMessagePackFormatter
	{
		public void Serialize(ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
		{
			TypelessFormatter.Instance.Serialize(ref writer, value, options);
		}

		public T? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			return (T)TypelessFormatter.Instance.Deserialize(ref reader, options);
		}
	}
}
