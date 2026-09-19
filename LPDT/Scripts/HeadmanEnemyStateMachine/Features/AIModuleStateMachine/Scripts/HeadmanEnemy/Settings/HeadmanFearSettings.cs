using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadmanFearSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headman/HeadmanFearSettings")]
	public class HeadmanFearSettings : ScriptableObject
	{
		[field: SerializeField]
		public float FearSpeed { get; private set; } = 6f;

		[field: SerializeField]
		public float FearDestinationUpdateInterval { get; private set; } = 0.2f;

		[field: SerializeField]
		public float FearRunDistance { get; private set; } = 12f;

		[field: SerializeField]
		public float FearDestinationReachedDistance { get; private set; } = 5f;

		[field: SerializeField]
		public float FearDetectionDistance { get; private set; } = 10f;
	}
}
