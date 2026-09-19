using System;
using System.Diagnostics;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = true)]
	[Conditional("NEVERDEFINED")]
	public class MessagePackKnownFormatterAttribute : Attribute
	{
		public Type FormatterType { get; }

		public MessagePackKnownFormatterAttribute(Type formatterType)
		{
			FormatterType = formatterType;
		}
	}
}
