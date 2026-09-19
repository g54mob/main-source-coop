namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	public enum SleeperEvent
	{
		None = 0,
		OnSoundHeard = 1,
		OnTargetAcquired = 2,
		OnAggroTimeout = 3,
		OnInAttackRange = 4,
		OnChaseTimeout = 5,
		OnAttackFinished = 6,
		OnReachedHome = 7,
		OnDamaged = 8,
		OnReachedSoundPoint = 9,
		OnWakeUpFinished = 10,
		OnDamageAggroTimeout = 11,
		OnFear = 12,
		OnFearEscapeCompleted = 13,
		OnInvestigateEmpty = 14
	}
}
