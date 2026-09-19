using System;

namespace Fusion
{
	[Flags]
	public enum NetworkObjectFlags
	{
		None = 0,
		MaskVersion = 0xFF,
		V1 = 1,
		V2 = 2,
		HasMainNetworkTRSP = 0x100,
		Ignore = 0x10000,
		MasterClientObject = 0x20000,
		DestroyWhenStateAuthorityLeaves = 0x40000,
		AllowStateAuthorityOverride = 0x80000,
		EnableAreaOfInterest = 0x100000,
		EnableExplicitObjectInterest = 0x200000
	}
}
