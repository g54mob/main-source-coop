using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct NetworkObjectBandwidthSample
	{
		[Key(0)]
		public int SessionTimeMs { get; set; }

		[Key(1)]
		public string ObjectName { get; set; }

		[Key(2)]
		public float InBandwidth { get; set; }

		[Key(3)]
		public float OutBandwidth { get; set; }
	}
}
