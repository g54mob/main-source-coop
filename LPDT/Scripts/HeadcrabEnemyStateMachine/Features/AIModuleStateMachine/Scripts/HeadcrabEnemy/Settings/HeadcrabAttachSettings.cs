using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadcrabAttachSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headcrab/HeadcrabAttachSettings")]
	public class HeadcrabAttachSettings : ScriptableObject
	{
		[field: SerializeField]
		public float TimeBeforeDeattaching { get; private set; } = 0.5f;

		[field: SerializeField]
		public float TimeToMoveToPosition { get; private set; } = 1f;

		[field: SerializeField]
		public float TimeBeforeAttaching { get; private set; } = 0.5f;

		[field: SerializeField]
		public float SnappingTime { get; private set; } = 10f;

		[field: SerializeField]
		public float CauldronedSittingTime { get; private set; } = 10f;

		[field: SerializeField]
		public float ResnappingCooldown { get; private set; } = 4f;

		[field: SerializeField]
		public float DistanceToGoHome { get; private set; } = 2f;
	}
}
