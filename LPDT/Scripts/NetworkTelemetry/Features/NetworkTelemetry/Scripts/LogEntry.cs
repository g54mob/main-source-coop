using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct LogEntry
	{
		[Key(0)]
		public long LocalTimeMs { get; set; }

		[Key(1)]
		public int SessionTimeMs { get; set; }

		[Key(2)]
		public string Level { get; set; }

		[Key(3)]
		public string Message { get; set; }
	}
}
