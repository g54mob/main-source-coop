namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public enum RatsHoleEnemyEvent
	{
		None = 0,
		OnTargetAcquired = 1,
		OnInAttackRange = 3,
		OnOutOfAttackRange = 4,
		OnDamaged = 5,
		OnFear = 6,
		OnNoMigrationTargetFound = 10,
		OnNoTargetForPatrol = 11,
		OnPatrolTimeout = 12
	}
}
