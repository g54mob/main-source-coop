using System;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[Serializable]
	public struct CustomPlayerEventRequest
	{
		public int OwnerPlayerId;

		public string EvenName;

		public AnalyticsEventQuotaGroup QuotaGroup;
	}
}
