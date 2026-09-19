using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core
{
	public sealed class AnalyticsEventQuota
	{
		private readonly SentAnalyticsModel _sentAnalyticsModel;

		private readonly GameAnalyticsEventsConfiguration _configuration;

		public AnalyticsEventQuota(SentAnalyticsModel sentAnalyticsModel, GameAnalyticsEventsConfiguration configuration)
		{
			_sentAnalyticsModel = sentAnalyticsModel;
			_configuration = configuration;
		}

		public bool CanSendBatch(AnalyticsEventQuotaGroup quotaGroup, int count)
		{
			if (count <= 0)
			{
				return true;
			}
			return count <= GetRemaining(quotaGroup);
		}

		public bool HasRemaining(AnalyticsEventQuotaGroup quotaGroup)
		{
			return GetRemaining(quotaGroup) > 0;
		}

		public int GetRemaining(AnalyticsEventQuotaGroup quotaGroup)
		{
			return _configuration.GetQuotaLimit(quotaGroup) - _sentAnalyticsModel.GetQuotaCount(quotaGroup);
		}

		public void RecordSent(AnalyticsEventQuotaGroup quotaGroup, int count = 1)
		{
			if (count > 0)
			{
				_sentAnalyticsModel.IncrementQuotaCount(quotaGroup, count);
			}
		}
	}
}
