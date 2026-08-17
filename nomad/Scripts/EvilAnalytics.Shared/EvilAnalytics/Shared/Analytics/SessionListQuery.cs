using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class SessionListQuery : AnalyticsQueryBase
	{
		public Guid? PlayerId { get; set; }

		public TimeSpan? MinDuration { get; set; }

		public TimeSpan? MaxDuration { get; set; }

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 50;
	}
}
