using System;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[Serializable]
	public struct StoreAnalyticsQuotaResponseData
	{
		public int PlayerId;

		public int SessionId;

		public bool HasEnoughQuota;
	}
}
