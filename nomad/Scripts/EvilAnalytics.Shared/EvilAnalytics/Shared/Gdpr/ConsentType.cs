using System;

namespace EvilAnalytics.Shared.Gdpr
{
	[Flags]
	public enum ConsentType
	{
		None = 0,
		Analytics = 1,
		Hardware = 2,
		Performance = 4,
		CrashReporting = 8,
		All = 0xF
	}
}
