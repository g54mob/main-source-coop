using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class ReorderPinnedEventsRequest
	{
		public List<Guid> OrderedIds { get; set; } = new List<Guid>();
	}
}
