using System;

namespace Rewired.Platforms.Microsoft.WindowsGamingInput
{
	[Flags]
	internal enum GamepadButtons : uint
	{
		None = 0u,
		Menu = 1u,
		View = 2u,
		A = 4u,
		B = 8u,
		X = 0x10u,
		Y = 0x20u,
		DPadUp = 0x40u,
		DPadDown = 0x80u,
		DPadLeft = 0x100u,
		DPadRight = 0x200u,
		LeftShoulder = 0x400u,
		RightShoulder = 0x800u,
		LeftThumbstick = 0x1000u,
		RightThumbstick = 0x2000u
	}
}
