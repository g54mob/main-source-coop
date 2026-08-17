using System;

namespace EvilAnalytics.Shared.Analytics
{
	public class ConsentInfo
	{
		public bool Analytics { get; set; }

		public bool Hardware { get; set; }

		public bool Performance { get; set; }

		public bool CrashReporting { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }
	}
}
