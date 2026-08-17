using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class PinnedEventDto
	{
		public Guid Id { get; set; }

		public string EventName { get; set; } = string.Empty;

		public string? EventCategory { get; set; }

		public string? DisplayName { get; set; }

		public int DisplayOrder { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
