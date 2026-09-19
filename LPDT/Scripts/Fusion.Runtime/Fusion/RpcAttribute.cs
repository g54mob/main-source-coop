using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public class RpcAttribute : RpcAttributeBase
	{
		public const int MaxPayloadSize = 512;

		public RpcSources Sources { get; } = RpcSources.All;

		public RpcTargets Targets { get; } = RpcTargets.All;

		public bool InvokeLocal
		{
			get
			{
				return InvokeLocalMode == RpcInvokeLocalMode.Immediate;
			}
			set
			{
				InvokeLocalMode = ((!value) ? RpcInvokeLocalMode.NotInvocable : RpcInvokeLocalMode.Immediate);
			}
		}

		public RpcInvokeLocalMode InvokeLocalMode { get; set; } = RpcInvokeLocalMode.Immediate;

		public RpcChannel Channel { get; set; } = RpcChannel.Reliable;

		public bool TickAligned { get; set; } = true;

		public uint Key { get; set; } = 0u;

		public RpcHostMode HostMode { get; set; } = RpcHostMode.SourceIsServer;

		public RpcAttribute()
		{
		}

		public RpcAttribute(RpcSources sources, RpcTargets targets)
		{
			Sources = sources;
			Targets = targets;
		}
	}
}
