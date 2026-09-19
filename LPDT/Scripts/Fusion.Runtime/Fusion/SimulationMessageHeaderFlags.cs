using System;

namespace Fusion
{
	[Flags]
	internal enum SimulationMessageHeaderFlags : byte
	{
		None = 0,
		Internal = 1,
		Rpc = 2,
		ReliableData = 4,
		AllowRoundtrip = 8,
		Reliable = 0x10,
		TickAligned = 0x20,
		HasTargetObject = 0x40,
		HasTargetPlayer = 0x80
	}
}
