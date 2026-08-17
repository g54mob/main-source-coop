using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.RemoteConfig
{
	public class LiveConfigResponse
	{
		public Dictionary<string, object> Configs { get; set; } = new Dictionary<string, object>();

		public int ConfigVersion { get; set; }

		public DateTimeOffset FetchedAt { get; set; }
	}
}
