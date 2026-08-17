using System.Collections.Generic;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class CrashlyticsOverview
	{
		public int TotalErrors { get; set; }

		public int TotalExceptions { get; set; }

		public int TotalWarnings { get; set; }

		public int UniqueErrors { get; set; }

		public int AffectedPlayers { get; set; }

		public int AffectedSessions { get; set; }

		public List<CrashLogSummary> TopErrors { get; set; } = new List<CrashLogSummary>();

		public List<ErrorTrendPoint> ErrorTrend { get; set; } = new List<ErrorTrendPoint>();
	}
}
