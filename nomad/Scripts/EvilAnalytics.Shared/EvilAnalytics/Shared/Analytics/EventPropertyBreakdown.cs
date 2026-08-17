using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class EventPropertyBreakdown
	{
		public string EventName { get; set; } = string.Empty;

		public string PropertyName { get; set; } = string.Empty;

		public List<PropertyValueCount> Values { get; set; } = new List<PropertyValueCount>();
	}
}
