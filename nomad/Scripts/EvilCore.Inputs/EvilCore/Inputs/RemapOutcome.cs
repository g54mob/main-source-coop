namespace EvilCore.Inputs
{
	public enum RemapOutcome
	{
		Applied = 0,
		Conflict = 1,
		Protected = 2,
		PendingConfirm = 3,
		Cancelled = 4,
		TimedOut = 5
	}
}
