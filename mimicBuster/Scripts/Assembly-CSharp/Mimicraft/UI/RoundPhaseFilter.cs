using System;

namespace Mimicraft.UI
{
	[Flags]
	public enum RoundPhaseFilter
	{
		None = 0,
		WaitingForPlayers = 1,
		Prep = 2,
		Hunt = 4,
		RoundEnd = 8,
		Everything = 0xF
	}
}
