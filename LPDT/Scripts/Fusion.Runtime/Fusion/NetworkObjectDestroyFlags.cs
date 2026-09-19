using System;

namespace Fusion
{
	[Flags]
	internal enum NetworkObjectDestroyFlags
	{
		None = 0,
		DestroyedByEngine = 1,
		DestroyState = 2,
		DestroyedByDespawn = 4,
		DestroyedByShutdown = 8,
		DestroyedByReplicator = 0x10
	}
}
