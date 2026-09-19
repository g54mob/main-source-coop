namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public enum MimicEvent
	{
		OnTargetAcquired = 0,
		OnTargetLost = 1,
		OnTargetPositionCompleted = 2,
		OnSimulationComplete = 3,
		OnReadyForAttack = 4,
		OnAttackCooldown = 5,
		OnFear = 6,
		OnFearEscapeCompleted = 7,
		OnDespawnRequested = 8,
		OnAttractionZoneEntered = 9,
		OnAttractionZoneExited = 10
	}
}
