using System.Collections.Generic;

namespace Features.NetworkTelemetry.Scripts
{
	public interface ITelemetryService
	{
		void RecordEvent(string name, Dictionary<string, string> extras = null);

		void RecordItemInteractionEvent(string itemType);
	}
}
