using System;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class MessagePackObjectAttribute : Attribute
	{
		public bool KeyAsPropertyName { get; }

		public bool SuppressSourceGeneration { get; set; }

		public bool AllowPrivate { get; set; }

		public MessagePackObjectAttribute(bool keyAsPropertyName = false)
		{
			KeyAsPropertyName = keyAsPropertyName;
		}
	}
}
