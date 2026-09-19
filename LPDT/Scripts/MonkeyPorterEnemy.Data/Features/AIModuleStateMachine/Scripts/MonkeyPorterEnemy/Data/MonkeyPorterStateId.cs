namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data
{
	public enum MonkeyPorterStateId
	{
		None = 0,
		Idle = 1,
		GoToPlayers = 2,
		WaitForItem = 3,
		CarryToBoat = 4,
		Deliver = 5,
		Blocked = 7,
		Flee = 8
	}
}
