namespace Rewired
{
	[CustomObfuscation(rename = false)]
	internal enum ControlDeviceType
	{
		Keyboard = 0,
		Mouse = 1,
		Joystick = 2,
		Gamepad = 3,
		ArcadeStick = 4,
		DancePad = 5,
		DrumKit = 6,
		Flight = 7,
		Throttle = 8,
		Guitar = 9,
		Wheel = 10,
		Paddle = 11,
		Custom = 99,
		Unknown = 100
	}
}
