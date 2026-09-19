using System;

namespace Features.PhysicsVolumeModule.Scripts
{
	[Flags]
	public enum ConstraintBoxFace
	{
		None = 0,
		PositiveX = 1,
		NegativeX = 2,
		PositiveY = 4,
		NegativeY = 8,
		PositiveZ = 0x10,
		NegativeZ = 0x20
	}
}
