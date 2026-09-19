using System.Collections.Generic;

namespace Features.UserReport.CustomUserReporting.Scripts.Client
{
	public struct UserReportMeasure
	{
		public int EndFrameNumber { get; set; }

		public List<UserReportNamedValue> Metadata { get; set; }

		public List<UserReportMetric> Metrics { get; set; }

		public int StartFrameNumber { get; set; }
	}
}
