using System;

namespace EvilAnalytics.Shared.Events
{
	public class HeartbeatResponse
	{
		public bool Success { get; set; }

		public DateTimeOffset ServerTime { get; set; }
	}
}
