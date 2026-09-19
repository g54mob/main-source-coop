using System.Collections.Generic;
using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public class TelemetryPacket
	{
		[Key(0)]
		public string SessionId { get; set; }

		[Key(1)]
		public string PlayerId { get; set; }

		[Key(2)]
		public string SessionPlayerId { get; set; }

		[Key(3)]
		public List<PositionSample> Positions { get; } = new List<PositionSample>(64);

		[Key(4)]
		public List<LogEntry> Logs { get; } = new List<LogEntry>(32);

		[Key(5)]
		public List<TelemetryEvent> Events { get; } = new List<TelemetryEvent>(16);

		[Key(6)]
		public List<FusionTelemetryStatSample> FusionTelemetryStats { get; } = new List<FusionTelemetryStatSample>(8);

		[Key(7)]
		public List<EnemyPositionSample> EnemyPositions { get; } = new List<EnemyPositionSample>(32);

		[Key(8)]
		public List<GrabObjectPositionSample> GrabObjectPositions { get; } = new List<GrabObjectPositionSample>(32);

		[Key(9)]
		public List<FrameTimingSample> FrameTimingSamples { get; } = new List<FrameTimingSample>(256);

		[Key(10)]
		public List<NetworkObjectBandwidthSample> NetworkObjectBandwidthSamples { get; } = new List<NetworkObjectBandwidthSample>(256);

		[Key(11)]
		public string GitCommitShort { get; set; }

		[Key(12)]
		public string AppVersion { get; set; }

		[IgnoreMember]
		public bool IsEmpty
		{
			get
			{
				if (Positions.Count == 0 && Logs.Count == 0 && Events.Count == 0 && FusionTelemetryStats.Count == 0 && EnemyPositions.Count == 0 && GrabObjectPositions.Count == 0 && FrameTimingSamples.Count == 0)
				{
					return NetworkObjectBandwidthSamples.Count == 0;
				}
				return false;
			}
		}

		public void Clear()
		{
			Positions.Clear();
			FusionTelemetryStats.Clear();
			EnemyPositions.Clear();
			GrabObjectPositions.Clear();
			FrameTimingSamples.Clear();
			NetworkObjectBandwidthSamples.Clear();
			Logs.Clear();
			Events.Clear();
			SessionId = null;
			PlayerId = null;
			SessionPlayerId = null;
			GitCommitShort = null;
			AppVersion = null;
		}
	}
}
