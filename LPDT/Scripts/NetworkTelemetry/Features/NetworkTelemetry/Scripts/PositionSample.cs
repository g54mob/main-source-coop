using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct PositionSample
	{
		[Key(0)]
		public int SessionTimeMs { get; set; }

		[Key(1)]
		public float X { get; set; }

		[Key(2)]
		public float Y { get; set; }

		[Key(3)]
		public float Z { get; set; }

		[Key(4)]
		public float Yaw { get; set; }
	}
}
