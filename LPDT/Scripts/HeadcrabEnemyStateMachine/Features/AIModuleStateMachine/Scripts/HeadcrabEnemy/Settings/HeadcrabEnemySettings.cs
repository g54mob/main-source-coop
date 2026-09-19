using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadcrabEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/Headcrab/HeadcrabEnemySettings")]
	public class HeadcrabEnemySettings : ScriptableObject
	{
		[field: SerializeField]
		public float Damage { get; private set; } = 100f;

		[field: SerializeField]
		public float DamageImpulseStrength { get; private set; } = 5f;

		[field: SerializeField]
		public float HomeSpawnRadius { get; private set; } = 10f;

		[field: SerializeField]
		public float HomeSpawnPathLength { get; private set; } = 20f;

		[field: SerializeField]
		public float CeilingRaycastDistance { get; private set; } = 30f;

		[field: SerializeField]
		public int HomeSpawnAttempts { get; private set; } = 15;
	}
}
