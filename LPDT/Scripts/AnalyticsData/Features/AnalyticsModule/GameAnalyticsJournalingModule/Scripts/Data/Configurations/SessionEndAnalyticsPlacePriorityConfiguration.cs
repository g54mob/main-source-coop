using Global.SerializableDictionary;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations
{
	[CreateAssetMenu(fileName = "SessionEndAnalyticsPlacePriorityConfiguration_Default", menuName = "Configurations/GameAnalyticsJournalingModule/SessionEndAnalyticsPlacePriorityConfiguration")]
	public class SessionEndAnalyticsPlacePriorityConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<SessionEndAnalyticsPlaceKind, int> PlacePriorities { get; private set; } = new SerializableDictionary<SessionEndAnalyticsPlaceKind, int>();

		public int GetPriority(SessionEndAnalyticsPlaceKind kind)
		{
			if (kind == SessionEndAnalyticsPlaceKind.Unknown)
			{
				return 0;
			}
			if (PlacePriorities != null && PlacePriorities.TryGetValue(kind, out var value))
			{
				return value;
			}
			Debug.LogError(string.Format("{0} missing priority for {1}.", "SessionEndAnalyticsPlacePriorityConfiguration", kind), this);
			return 0;
		}
	}
}
