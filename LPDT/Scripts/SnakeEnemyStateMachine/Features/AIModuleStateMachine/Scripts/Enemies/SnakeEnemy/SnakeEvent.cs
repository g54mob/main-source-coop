namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	public enum SnakeEvent
	{
		None = 0,
		OnTargetAcquired = 1,
		OnTargetLost = 2,
		OnDamaged = 5,
		OnFear = 6,
		OnFearEscapeCompleted = 7,
		OnSteppedOn = 8,
		OnStepAggroDisengaged = 9,
		OnReachedWrapTarget = 10,
		OnWrapFinished = 11,
		OnSafeZoneApproach = 12,
		OnSafeZoneApproachFinished = 13
	}
}
