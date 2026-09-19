namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	public enum EarEvent
	{
		None = 0,
		OnSoundHeard = 1,
		OnReachedSoundPoint = 2,
		OnAttackFinished = 3,
		OnFear = 4,
		OnFearEscapeCompleted = 5,
		OnStunFinished = 6,
		OnSoundTargetLost = 7,
		OnNewAreaReached = 8,
		OnPostAttackWanderFinished = 9
	}
}
