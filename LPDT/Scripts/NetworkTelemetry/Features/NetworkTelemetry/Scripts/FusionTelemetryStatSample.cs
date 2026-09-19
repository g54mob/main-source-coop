using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct FusionTelemetryStatSample
	{
		[Key(0)]
		public int SessionTimeMs { get; set; }

		[Key(1)]
		public float LocalRttSeconds { get; set; }

		[Key(2)]
		public float CloudRttSeconds { get; set; }

		[Key(3)]
		public int Player1Id { get; set; }

		[Key(4)]
		public float Player1Rtt { get; set; }

		[Key(5)]
		public int Player2Id { get; set; }

		[Key(6)]
		public float Player2Rtt { get; set; }

		[Key(7)]
		public int Player3Id { get; set; }

		[Key(8)]
		public float Player3Rtt { get; set; }

		[Key(9)]
		public int Player4Id { get; set; }

		[Key(10)]
		public float Player4Rtt { get; set; }

		[Key(11)]
		public long PhotonBytesInTotal { get; set; }

		[Key(12)]
		public long PhotonBytesOutTotal { get; set; }

		[Key(13)]
		public long PhotonBytesInDelta { get; set; }

		[Key(14)]
		public long PhotonBytesOutDelta { get; set; }

		[Key(15)]
		public int PhotonPeerRttMs { get; set; }

		[Key(16)]
		public long PhotonPacketLossByCrc { get; set; }

		[Key(17)]
		public long PhotonResentReliableCommands { get; set; }
	}
}
