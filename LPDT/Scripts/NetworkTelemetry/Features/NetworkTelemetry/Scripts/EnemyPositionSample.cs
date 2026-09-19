using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct EnemyPositionSample
	{
		[Key(0)]
		public int SessionTimeMs { get; set; }

		[Key(1)]
		public string EnemyKind { get; set; }

		[Key(2)]
		public string NetworkObjectId { get; set; }

		[Key(3)]
		public float X { get; set; }

		[Key(4)]
		public float Y { get; set; }

		[Key(5)]
		public float Z { get; set; }

		[Key(6)]
		public float Yaw { get; set; }

		[Key(7)]
		public int CurrentState { get; set; }
	}
}
