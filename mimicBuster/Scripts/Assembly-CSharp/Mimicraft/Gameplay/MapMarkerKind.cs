using System;

namespace Mimicraft.Gameplay
{
	[Flags]
	public enum MapMarkerKind
	{
		None = 0,
		HunterSpawn = 1,
		HiderSpawn = 2,
		HunterDoor = 4,
		LobbySpawn = 8,
		ModelerSpawn = 0x10,
		HunterRoomSpawn = 0x20,
		ArenaSpawn = 0x40
	}
}
