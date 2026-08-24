namespace FishNet.Managing.Server
{
	public enum KickReason : short
	{
		Unset = 0,
		ExploitAttempt = 1,
		MalformedData = 2,
		ExploitExcessiveData = 3,
		ExcessiveData = 4,
		UnexpectedProblem = 5,
		UnusualActivity = 6
	}
}
