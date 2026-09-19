using System;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	[Serializable]
	public class BigButtBeachCounterProgressEntry
	{
		public string StatAPIString;

		public long CurrentValue;

		public long SteamMaxValue;

		public long SavedUtcTicks;
	}
}
