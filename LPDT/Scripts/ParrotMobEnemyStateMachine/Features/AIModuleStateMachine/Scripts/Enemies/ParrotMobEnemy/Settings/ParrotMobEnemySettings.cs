using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings
{
	[CreateAssetMenu(fileName = "ParrotMobEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/ParrotMob/ParrotMobEnemySettings")]
	public class ParrotMobEnemySettings : ScriptableObject
	{
		[field: SerializeField]
		public float MoveSpeed { get; private set; } = 1.5f;

		[field: SerializeField]
		public float CompletePointMinDistance { get; private set; } = 0.5f;

		[field: SerializeField]
		public float DetectionRange { get; private set; } = 8f;

		[field: SerializeField]
		public ParrotMobLookYawRange[] AllowedLookYawRanges { get; private set; } = new ParrotMobLookYawRange[2]
		{
			new ParrotMobLookYawRange(20f, 140f),
			new ParrotMobLookYawRange(200f, 340f)
		};
	}
}
