using System;

namespace Fusion.Sockets.V2
{
	[Flags]
	internal enum ChannelFlags : byte
	{
		Reliable = 1,
		NotifyDelivered = 2
	}
}
