namespace Features.AIModuleStateMachine.Scripts.Core.Enemy
{
	public enum SimpleEnemyEvent
	{
		None = 0,
		OnTargetAcquired = 1,
		OnTargetLost = 2,
		OnInAttackRange = 3,
		OnOutOfAttackRange = 4
	}
}
