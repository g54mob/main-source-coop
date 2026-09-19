using System;
using UnityEngine.Serialization;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[Serializable]
	public class AnalyticsQuotaCountEntry
	{
		[FormerlySerializedAs("Kind")]
		public int Group;

		public int Count;
	}
}
