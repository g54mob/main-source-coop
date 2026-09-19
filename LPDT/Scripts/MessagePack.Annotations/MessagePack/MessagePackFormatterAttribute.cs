using System;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
	public class MessagePackFormatterAttribute : Attribute
	{
		public Type FormatterType { get; }

		public object?[]? Arguments { get; }

		public MessagePackFormatterAttribute(Type formatterType)
		{
			FormatterType = formatterType ?? throw new ArgumentNullException("formatterType");
		}

		public MessagePackFormatterAttribute(Type formatterType, params object?[]? arguments)
		{
			FormatterType = formatterType ?? throw new ArgumentNullException("formatterType");
			Arguments = arguments;
		}
	}
}
