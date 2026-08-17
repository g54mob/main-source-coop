using System;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class LogBatchResponse
	{
		public int Accepted { get; set; }

		public int Rejected { get; set; }

		public DateTimeOffset ServerTime { get; set; }
	}
}
