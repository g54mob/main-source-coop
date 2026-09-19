namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	public enum AnchorEvent
	{
		None = 0,
		OnTargetAcquired = 1,
		OnTargetLost = 2,
		OnThrowStarted = 3,
		OnAnchorHitPlayer = 4,
		OnAnchorMissed = 5,
		OnPlayerGrabbed = 6,
		OnGrabReleased = 7,
		OnReleaseCompleted = 8,
		OnDamaged = 9,
		OnDamageAggroTimeout = 10,
		OnFear = 11,
		OnFearEscapeCompleted = 12,
		OnMeleeAttack = 13,
		OnMeleeCompleted = 14,
		OnGrabEscaped = 15,
		OnAttractionZoneEntered = 16,
		OnAttractionZoneExited = 17,
		OnThrowCancelled = 18
	}
}
