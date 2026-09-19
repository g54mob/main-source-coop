namespace Features.AIModuleStateMachine.Scripts.SharkEnemy
{
	public enum SharkEvent
	{
		OnTargetAcquired = 0,
		OnTargetLost = 1,
		OnAttackFinished = 2,
		OnFear = 3,
		OnFinDamaged = 4,
		OnCommitSimpleAttack = 5,
		OnCommitLethalAttack = 6,
		OnHuntWithdraw = 7,
		OnMimicTargetAcquired = 8,
		OnAttractionZoneEntered = 9,
		OnAttractionZoneExited = 10
	}
}
