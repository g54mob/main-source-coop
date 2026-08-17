using System;

namespace Ami.BroAudio
{
	[Flags]
	public enum EffectType
	{
		None = 0,
		Volume = 1,
		LowPass = 2,
		HighPass = 4,
		Custom = 8,
		All = 0xF
	}
}
