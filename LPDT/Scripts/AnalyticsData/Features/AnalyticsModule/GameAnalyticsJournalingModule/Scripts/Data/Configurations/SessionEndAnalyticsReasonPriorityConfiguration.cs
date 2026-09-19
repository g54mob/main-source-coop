using Global.SerializableDictionary;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations
{
	[CreateAssetMenu(fileName = "SessionEndAnalyticsReasonPriorityConfiguration_Default", menuName = "Configurations/GameAnalyticsJournalingModule/SessionEndAnalyticsReasonPriorityConfiguration")]
	public class SessionEndAnalyticsReasonPriorityConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<SessionEndAnalyticsReason, int> ReasonPriorities { get; private set; } = new SerializableDictionary<SessionEndAnalyticsReason, int>();

		public int GetPriority(SessionEndAnalyticsReason reason)
		{
			if (reason == SessionEndAnalyticsReason.Unknown)
			{
				return 0;
			}
			if (ReasonPriorities != null && ReasonPriorities.TryGetValue(reason, out var value))
			{
				return value;
			}
			Debug.LogError(string.Format("{0} missing priority for {1}.", "SessionEndAnalyticsReasonPriorityConfiguration", reason), this);
			return 0;
		}
	}
}
