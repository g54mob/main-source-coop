using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Settings
{
	[CreateAssetMenu(fileName = "RatsHoleEnemyAttackSettings_Default", menuName = "Configurations/AIModuleStateMachine/RatsHoleEnemy/RatsHoleEnemyAttackSettings")]
	public class RatsHoleEnemyAttackSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AttackRange { get; private set; } = 2f;

		[field: SerializeField]
		public float MaxAttackVerticalDelta { get; private set; } = 2.5f;

		[field: SerializeField]
		public float AttackFrequency { get; private set; } = 1f;

		[field: SerializeField]
		public float Damage { get; private set; } = 10f;

		[field: SerializeField]
		public float ForceStrength { get; private set; } = 2f;
	}
}
