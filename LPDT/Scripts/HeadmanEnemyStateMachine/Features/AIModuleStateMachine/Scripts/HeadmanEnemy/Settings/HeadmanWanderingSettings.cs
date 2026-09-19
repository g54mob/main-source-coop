using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadmanWanderingSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headman/HeadmanWanderingSettings")]
	public class HeadmanWanderingSettings : ScriptableObject
	{
		[field: SerializeField]
		public float WanderRadius { get; private set; } = 10f;

		[field: SerializeField]
		public float WanderingSpeed { get; private set; } = 3f;

		[field: SerializeField]
		public float MinWanderPositionUpdateTime { get; private set; } = 5f;

		[field: SerializeField]
		public float MaxWanderPositionUpdateTime { get; private set; } = 10f;

		[field: SerializeField]
		public float ChangeLocationUpdateFrequency { get; private set; } = 45f;
	}
}
