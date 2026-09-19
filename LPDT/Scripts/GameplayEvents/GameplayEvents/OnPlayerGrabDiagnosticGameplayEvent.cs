using System.Collections.Generic;

namespace GameplayEvents
{
	public class OnPlayerGrabDiagnosticGameplayEvent : GameplayEvent
	{
		public readonly Dictionary<string, string> Extras;

		public OnPlayerGrabDiagnosticGameplayEvent(Dictionary<string, string> extras)
		{
			Extras = extras;
		}
	}
}
