using System;

namespace Features.ExtendedLogger.Scripts
{
	[Flags]
	public enum DebugFilterType
	{
		None = 0,
		UI = 1,
		Gameplay = 2,
		Network = 4,
		Audio = 8,
		AI = 0x10,
		EnemiesSpawn = 0x20,
		Analytics = 0x40,
		PlatformStatus = 0x80,
		Tutorial = 0x100,
		All = -1
	}
}
