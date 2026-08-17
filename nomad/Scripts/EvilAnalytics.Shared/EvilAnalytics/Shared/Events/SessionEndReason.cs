namespace EvilAnalytics.Shared.Events
{
	public enum SessionEndReason
	{
		Normal = 0,
		Crash = 1,
		Timeout = 2,
		ForceQuit = 3,
		Unknown = 99
	}
}
