using System;
using System.Collections.Generic;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[Serializable]
	public class SentAnalyticsDataHolder
	{
		public long DateTimeTicks;

		public int EventCount;

		public int SessionStartedCount;

		public List<AnalyticsQuotaCountEntry> QuotaCounts = new List<AnalyticsQuotaCountEntry>();
	}
}
