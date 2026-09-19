namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy
{
	public enum CrabEvent
	{
		None = 0,
		OnTargetAcquired = 1,
		OnTargetLost = 2,
		OnGrabbed = 3,
		OnFear = 4,
		OnFearEscapeCompleted = 5,
		OnItemPickedUp = 6,
		OnItemWanderCompleted = 7,
		OnPlayerWanderCompleted = 8,
		OnItemDropped = 9,
		OnItemLost = 10,
		OnRestCompleted = 11,
		OnNewPosReached = 12,
		OnAttractionZoneEntered = 13,
		OnAttractionZoneExited = 14,
		OnDamaged = 15,
		OnDamageAggroTimeout = 16,
		OnDetachCompleted = 17,
		OnPlayerReleasedEarly = 18,
		OnGrabbedPlayerDisconnected = 19,
		OnHoldTakenByOther = 20
	}
}
