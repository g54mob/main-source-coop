using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings
{
	[CreateAssetMenu(fileName = "SirenChaseSettings_Default", menuName = "Configurations/AIModuleStateMachine/Siren/SirenChaseSettings")]
	public class SirenChaseSettings : ScriptableObject
	{
		[field: SerializeField]
		public float MinChaseTimeBeforeAttack { get; private set; } = 0.5f;

		[field: SerializeField]
		public float MinLookLostTime { get; private set; }

		[field: SerializeField]
		public float MaxChasingTime { get; private set; } = 10f;

		[field: SerializeField]
		public float DistanceToAttack { get; private set; }
	}
}
