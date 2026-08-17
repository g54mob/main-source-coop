using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class OverviewMetrics
	{
		public AnalyticsPeriod Period { get; set; }

		public int DailyActiveUsers { get; set; }

		public int MonthlyActiveUsers { get; set; }

		public int NewPlayers { get; set; }

		public int TotalSessions { get; set; }

		public long TotalEvents { get; set; }

		public TimeSpan AverageSessionDuration { get; set; }

		public TimeSpan AveragePlaytimePerPlayer { get; set; }

		public double AverageSessionsPerUser { get; set; }

		public double RetentionDay1 { get; set; }

		public double RetentionDay7 { get; set; }

		public double RetentionDay30 { get; set; }

		public double RetentionDay90 { get; set; }

		public double RetentionDay180 { get; set; }

		public double RetentionDay360 { get; set; }

		public int PeakConcurrentPlayers { get; set; }

		public RetentionCohort RetentionCohorts { get; set; } = new RetentionCohort();
	}
}
