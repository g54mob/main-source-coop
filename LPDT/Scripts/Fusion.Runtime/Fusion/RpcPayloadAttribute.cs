using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class RpcPayloadAttribute : Attribute
	{
		public int ByteCount { get; }

		public RpcPayloadAttribute()
		{
		}

		public RpcPayloadAttribute(int byteSize)
		{
			ByteCount = byteSize;
		}
	}
}
