namespace EvilAnalytics.Shared.Analytics
{
	public class CreatePinnedEventRequest
	{
		public string EventName { get; set; } = string.Empty;

		public string? EventCategory { get; set; }

		public string? DisplayName { get; set; }
	}
}
