using Features.StoreModule.Scripts;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations
{
	[CreateAssetMenu(fileName = "TutorialStepsConfiguration_Default", menuName = "Configurations/TutorialModule/TutorialStepsConfiguration")]
	public class TutorialStepsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float LocationContainerOpenThreshold { get; private set; }

		[field: SerializeField]
		public uint QuotaInContainerThreshold { get; private set; }

		[field: SerializeField]
		public float PirateSpawnGapAfterPlayer { get; private set; }

		[field: SerializeField]
		public float TutorialCompleteStepDuration { get; private set; }

		[field: SerializeField]
		public uint CardsCountToChooseAsTip { get; private set; }

		[field: SerializeField]
		public uint CardsToBuyCount { get; private set; }

		[field: SerializeField]
		public CardItemType InteractedCardItemType { get; private set; }

		[field: SerializeField]
		public float ReadyDelayIfCardAutomaticallySubmitted { get; private set; }

		[field: SerializeField]
		public float DirectionAssistSpotAngle { get; private set; }
	}
}
