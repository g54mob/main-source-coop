namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public enum MimicStateId
	{
		None = 0,
		MoveToRandomPos = 1,
		MoveToPlayerArea = 2,
		PlayerBehaviorSimulation = 3,
		TargetChasing = 4,
		MoveToNewArea = 5,
		FearEscape = 6,
		Despawn = 7,
		AttractionInvestigate = 8
	}
}
