namespace MessagePack.Internal
{
	internal class MessagePackDynamicObjectResolverException : MessagePackSerializationException
	{
		internal MessagePackDynamicObjectResolverException(string message)
			: base(message)
		{
		}
	}
}
