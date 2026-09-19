using System.Collections.Generic;
using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct TelemetryEvent
	{
		[Key(0)]
		public long LocalTimeMs { get; set; }

		[Key(1)]
		public int SessionTimeMs { get; set; }

		[Key(2)]
		public string Name { get; set; }

		[Key(3)]
		public Dictionary<string, string> Extras { get; set; }
	}
}
