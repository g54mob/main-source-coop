using System;
using System.Collections.Generic;
using EvilAnalytics.Shared.Common;

namespace EvilAnalytics.Shared.Events
{
	public class GameEvent
	{
		public Guid EventId { get; set; } = Guid.NewGuid();

		public string EventName { get; set; } = string.Empty;

		public EventCategory Category { get; set; }

		public Guid SessionId { get; set; }

		public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

		public Dictionary<string, object>? Properties { get; set; }

		public int? LevelId { get; set; }

		public decimal? Value { get; set; }
	}
}
