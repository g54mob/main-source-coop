namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public enum CoinRobSwarmEvent
	{
		OnChaseRequested = 0,
		OnReachChasePoint = 1,
		OnItemsMarkedForSteal = 2,
		OnPlayerAttackStarted = 3,
		OnPlayerAttackCancelled = 4,
		OnRunAway = 5,
		OnRunAwayComplete = 6,
		OnFear = 7,
		OnFearEscapeCompleted = 8,
		OnCauldroned = 9
	}
}
