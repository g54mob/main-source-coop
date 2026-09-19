using System;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations
{
	[CreateAssetMenu(fileName = "GameAnalyticsEventsConfiguration_Default", menuName = "Configurations/GameAnalyticsJournalingModule/GameAnalyticsEventsConfiguration")]
	public class GameAnalyticsEventsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public int MaxEventsCountPerDay { get; private set; } = 500;

		[field: SerializeField]
		public int ReservedEventsCount { get; private set; } = 50;

		[field: SerializeField]
		public int SaveEveryNEvents { get; private set; } = 10;

		[field: SerializeField]
		public SerializableDictionary<AnalyticsEventQuotaGroup, int> QuotaLimitsPerDay { get; private set; } = new SerializableDictionary<AnalyticsEventQuotaGroup, int>();

		public int GetQuotaLimit(AnalyticsEventQuotaGroup quotaGroup)
		{
			if (QuotaLimitsPerDay != null && QuotaLimitsPerDay.TryGetValue(quotaGroup, out var value))
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("quotaGroup", quotaGroup, "Quota limit is not configured in QuotaLimitsPerDay.");
		}
	}
}
