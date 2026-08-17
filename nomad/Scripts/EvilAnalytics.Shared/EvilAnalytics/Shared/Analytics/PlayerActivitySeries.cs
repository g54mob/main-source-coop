using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerActivitySeries
	{
		public AnalyticsPeriod Period { get; set; } = new AnalyticsPeriod();

		public TimeGranularity Granularity { get; set; }

		public List<TimeSeriesDataPoint> ActiveUsers { get; set; } = new List<TimeSeriesDataPoint>();

		public List<TimeSeriesDataPoint> NewUsers { get; set; } = new List<TimeSeriesDataPoint>();

		public List<TimeSeriesDataPoint> Sessions { get; set; } = new List<TimeSeriesDataPoint>();
	}
}
