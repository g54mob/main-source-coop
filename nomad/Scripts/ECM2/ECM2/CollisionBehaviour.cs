using System;

namespace ECM2
{
	[Flags]
	public enum CollisionBehaviour
	{
		Default = 0,
		Walkable = 1,
		NotWalkable = 2,
		CanPerchOn = 4,
		CanNotPerchOn = 8,
		CanStepOn = 0x10,
		CanNotStepOn = 0x20,
		CanRideOn = 0x40,
		CanNotRideOn = 0x80
	}
}
