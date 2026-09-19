namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public enum PirateEvent
	{
		OnDamage = 0,
		OnFear = 1,
		OnTargetAcquired = 2,
		OnTargetLost = 3,
		OnTargetInAttackRange = 4,
		OnMeleeAttack = 5,
		OnRangedAttack = 6,
		OnWaitToAttack = 7,
		OnTargetOutAttackRange = 8,
		OnIdle = 9,
		OnFlee = 10,
		OnSafeZoneAttack = 11,
		OnAttractionZoneEntered = 12,
		OnAttractionZoneExited = 13
	}
}
