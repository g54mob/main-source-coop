using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class PlayerProfile : PlayerSummary
	{
		public List<SessionSummary> RecentSessions { get; set; } = new List<SessionSummary>();

		public List<EventCount> TopEvents { get; set; } = new List<EventCount>();

		public HardwareSummary? Hardware { get; set; }

		public ConsentInfo? Consent { get; set; }
	}
}
