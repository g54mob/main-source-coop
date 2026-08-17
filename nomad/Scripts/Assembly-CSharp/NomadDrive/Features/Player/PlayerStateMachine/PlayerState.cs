namespace NomadDrive.Features.Player.PlayerStateMachine
{
	public enum PlayerState
	{
		Idle = 0,
		Walk = 1,
		Sprint = 2,
		CrouchedIdle = 3,
		CrouchedWalk = 4,
		CrouchedSprint = 5,
		Sit = 6,
		Jump = 7,
		Falling = 8,
		Landing = 9,
		Laying = 10,
		DownedLying = 11,
		DownedSitting = 12
	}
}
