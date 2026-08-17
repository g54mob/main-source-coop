using System;

[Flags]
internal enum KayonGyuXSdBKkUDsBAQknhQxwaB
{
	Duration = 1,
	SamplePeriod = 2,
	Gain = 4,
	TriggerButton = 8,
	TriggerRepeatInterval = 0x10,
	Axes = 0x20,
	Direction = 0x40,
	Envelope = 0x80,
	TypeSpecificParameters = 0x100,
	StartDelay = 0x200,
	AllExceptDelay = 0x1FF,
	All = 0x3FF,
	Start = 0x20000000,
	NoRestart = 0x40000000,
	NoDownload = int.MinValue,
	None = 0
}
