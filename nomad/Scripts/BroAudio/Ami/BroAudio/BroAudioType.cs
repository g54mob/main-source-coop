using System;

namespace Ami.BroAudio
{
	[Flags]
	public enum BroAudioType
	{
		None = 0,
		Music = 1,
		UI = 2,
		Ambience = 4,
		SFX = 8,
		VoiceOver = 0x10,
		All = 0x1F
	}
}
