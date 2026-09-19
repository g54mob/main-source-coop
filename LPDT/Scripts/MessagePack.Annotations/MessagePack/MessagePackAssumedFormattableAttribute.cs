using System;
using System.Diagnostics;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = true)]
	[Conditional("NEVERDEFINED")]
	public class MessagePackAssumedFormattableAttribute : Attribute
	{
		public Type FormattableType { get; }

		public MessagePackAssumedFormattableAttribute(Type formattableType)
		{
			FormattableType = formattableType;
		}
	}
}
