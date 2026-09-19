using System.Collections.Generic;

namespace Fusion.Statistics
{
	public class NetworkObjectStatisticsSnapshot
	{
		public Dictionary<NetworkId, Dictionary<FusionObjectStatType, float>> NetworkObjectStatistics = new Dictionary<NetworkId, Dictionary<FusionObjectStatType, float>>();
	}
}
