using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class AnalyticsQueryBase
	{
		public DateTimeOffset? From { get; set; }

		public DateTimeOffset? To { get; set; }
	}
}
