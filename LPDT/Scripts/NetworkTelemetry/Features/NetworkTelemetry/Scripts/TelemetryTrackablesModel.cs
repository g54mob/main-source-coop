using System.Collections.Generic;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryTrackablesModel
	{
		public readonly HashSet<TelemetryTrackable> Trackables = new HashSet<TelemetryTrackable>();
	}
}
