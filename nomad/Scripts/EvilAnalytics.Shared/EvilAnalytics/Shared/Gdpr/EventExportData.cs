using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Gdpr
{
	public class EventExportData
	{
		public Guid EventId { get; set; }

		public string EventName { get; set; } = string.Empty;

		public string Category { get; set; } = string.Empty;

		public DateTimeOffset Timestamp { get; set; }

		public Dictionary<string, object>? Properties { get; set; }
	}
}
