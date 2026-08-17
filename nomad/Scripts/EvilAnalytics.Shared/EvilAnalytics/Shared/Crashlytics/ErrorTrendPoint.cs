using System;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class ErrorTrendPoint
	{
		public DateTimeOffset Date { get; set; }

		public int ErrorCount { get; set; }

		public int ExceptionCount { get; set; }

		public int WarningCount { get; set; }
	}
}
