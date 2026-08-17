using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class EventListQuery : AnalyticsQueryBase
	{
		public Guid? SessionId { get; set; }

		public Guid? PlayerId { get; set; }

		public string? EventName { get; set; }

		public string? Category { get; set; }

		public int? LevelId { get; set; }

		public Dictionary<string, string>? PropertyFilter { get; set; }

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 100;

		public bool SortDescending { get; set; } = true;
	}
}
