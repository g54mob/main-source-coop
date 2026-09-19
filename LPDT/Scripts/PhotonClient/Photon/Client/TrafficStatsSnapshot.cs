using System;

namespace Photon.Client
{
	public class TrafficStatsSnapshot : TrafficStatsBase
	{
		public long SnapshotTimestamp;

		[Obsolete("Use SnapshotTimestamp (without incorrect uppercase Stamp).")]
		public long SnapshotTimeStamp
		{
			get
			{
				return SnapshotTimestamp;
			}
			set
			{
				SnapshotTimestamp = value;
			}
		}

		public TrafficStatsSnapshot(TrafficStats ts, long timestamp)
			: base(ts)
		{
			SnapshotTimestamp = timestamp;
		}
	}
}
