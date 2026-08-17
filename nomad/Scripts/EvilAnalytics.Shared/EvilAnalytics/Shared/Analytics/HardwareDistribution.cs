using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class HardwareDistribution
	{
		public string Category { get; set; } = string.Empty;

		public List<DistributionValue> Values { get; set; } = new List<DistributionValue>();
	}
}
