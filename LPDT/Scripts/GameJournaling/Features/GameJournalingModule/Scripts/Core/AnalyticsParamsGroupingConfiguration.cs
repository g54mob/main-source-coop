using UnityEngine;

namespace Features.GameJournalingModule.Scripts.Core
{
	[CreateAssetMenu(fileName = "AnalyticsParamsGroupingConfiguration_Default", menuName = "Configurations/GameJournalingModule/AnalyticsParamsGroupingConfiguration")]
	public class AnalyticsParamsGroupingConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public int MaxSingleRunIndex { get; private set; }

		[field: SerializeField]
		public int GroupSize { get; private set; }

		[field: SerializeField]
		public int MaxShownRunIndex { get; private set; }
	}
}
