using UnityEngine;

namespace MessagePack.Unity
{
	public static class MessagePackInitializer
	{
		private static bool serializerRegistered;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
			if (!serializerRegistered)
			{
				MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard.WithResolver(UnityResolver.InstanceWithStandardResolver);
				serializerRegistered = true;
			}
		}
	}
}
