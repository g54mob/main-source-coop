namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public enum PirateStateId
	{
		Idle = 0,
		Chasing = 1,
		Combat = 2,
		WaitForAttack = 3,
		MeleeAttack = 4,
		RangedAttack = 5,
		Fleeing = 6,
		SafeZoneAttack = 7,
		Investigating = 8,
		AttractionInvestigate = 9
	}
}
