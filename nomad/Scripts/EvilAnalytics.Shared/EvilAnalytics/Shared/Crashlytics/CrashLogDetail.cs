using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class CrashLogDetail : CrashLogSummary
	{
		public Guid SessionId { get; set; }

		public string StackTrace { get; set; } = string.Empty;

		public float? CurrentFps { get; set; }

		public float? MemoryUsedMb { get; set; }

		public List<CrashLogOccurrence> RecentOccurrences { get; set; } = new List<CrashLogOccurrence>();
	}
}
