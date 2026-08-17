using System;

[Flags]
internal enum gxxFlTvKILunZojUxdgBOVTvPrAm
{
	Empty = 1,
	Stopped = 2,
	Paused = 4,
	ActuatorsOn = 0x10,
	ActuatorsOff = 0x20,
	PowerOn = 0x40,
	PowerOff = 0x80,
	SafetySwitchOn = 0x100,
	SafetySwitchOff = 0x200,
	UserSafetySwitchOn = 0x400,
	UserSafetySwitchOff = 0x800,
	DeviceLost = int.MinValue
}
