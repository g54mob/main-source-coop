namespace EvilAnalytics.Shared.Analytics
{
	public class EventPropertyQuery : AnalyticsQueryBase
	{
		public string EventName { get; set; } = string.Empty;

		public string PropertyName { get; set; } = string.Empty;

		public int Limit { get; set; } = 20;
	}
}
