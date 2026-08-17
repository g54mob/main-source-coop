using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class HardwareDistributionQuery : AnalyticsQueryBase
	{
		public List<string>? Categories { get; set; }

		public int Limit { get; set; } = 10;
	}
}
