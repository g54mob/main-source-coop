using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Crashlytics
{
	public class LogBatchRequest
	{
		public string DeviceId { get; set; } = string.Empty;

		public Guid SessionId { get; set; }

		public string GameVersion { get; set; }

		public string Platform { get; set; }

		public List<LogEntry> Entries { get; set; } = new List<LogEntry>();
	}
}
