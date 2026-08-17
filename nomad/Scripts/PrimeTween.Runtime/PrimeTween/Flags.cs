using System;

namespace PrimeTween
{
	[Flags]
	internal enum Flags
	{
		Additive = 1,
		ShakeSign = 2,
		ShakePunch = 4,
		WarnEndValueEqualsCurrent = 8,
		WarnIgnoredOnCompleteIfTargetDestroyed = 0x10,
		ResetOnComplete = 0x20,
		IsUpdating = 0x40,
		StoppedEmergently = 0x80,
		IsAlive = 0x100,
		StateBefore = 0x200,
		StateRunning = 0x400,
		StateAfter = 0x800,
		StartFromCurrent = 0x1000,
		IsDone = 0x2000,
		IsValueChanged = 0x4000,
		UseUnscaledTime = 0x8000,
		IsInSequence = 0x10000,
		IsPaused = 0x20000,
		IsUpdatedInJob = 0x40000,
		HasOnUpdate = 0x80000,
		IsSequenceInverted = 0x200000,
		IsCustomEaseSameStartEndValues = 0x400000
	}
}
