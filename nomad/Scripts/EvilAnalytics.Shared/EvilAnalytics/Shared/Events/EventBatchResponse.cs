using System.Collections.Generic;

namespace EvilAnalytics.Shared.Events
{
	public class EventBatchResponse
	{
		public int Accepted { get; set; }

		public int Rejected { get; set; }

		public List<string>? Errors { get; set; }
	}
}
