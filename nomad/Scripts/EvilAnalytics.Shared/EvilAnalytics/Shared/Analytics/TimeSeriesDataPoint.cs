using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class TimeSeriesDataPoint
	{
		public DateTimeOffset Timestamp { get; set; }

		public double Value { get; set; }
	}
}
