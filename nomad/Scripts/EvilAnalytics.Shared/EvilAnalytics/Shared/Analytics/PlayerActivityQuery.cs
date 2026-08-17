namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerActivityQuery : AnalyticsQueryBase
	{
		public TimeGranularity Granularity { get; set; } = TimeGranularity.Daily;
	}
}
