using System;

namespace Fusion.Sockets.V2
{
	[Flags]
	internal enum PacketTypeV2 : byte
	{
		NotifyData = 2,
		NotifyAcks = 4,
		Progress_Bit = 0x20,
		LastFrag_Bit = 0x80
	}
}
