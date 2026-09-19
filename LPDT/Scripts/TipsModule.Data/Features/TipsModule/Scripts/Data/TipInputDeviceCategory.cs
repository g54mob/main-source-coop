using System;

namespace Features.TipsModule.Scripts.Data
{
	[Flags]
	public enum TipInputDeviceCategory
	{
		None = 0,
		KeyboardAndMouse = 1,
		Gamepad = 2,
		Joystick = 4
	}
}
