using System;

namespace Features.TutorialModule.Scripts.GuideModule
{
	[Flags]
	public enum TipFollowFlags
	{
		None = 0,
		Position = 1,
		Rotation = 2,
		FaceCameraX = 4,
		FaceCameraY = 8,
		FaceCameraZ = 0x10,
		FaceCameraEntire = 0x1C
	}
}
