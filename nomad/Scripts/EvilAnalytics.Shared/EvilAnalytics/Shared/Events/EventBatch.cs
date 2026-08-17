using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Events
{
	public class EventBatch
	{
		public string DeviceId { get; set; } = string.Empty;

		[Obsolete("API key should be sent via X-API-Key header only. This field is ignored.")]
		public string ApiKey { get; set; } = string.Empty;

		public string SdkVersion { get; set; } = string.Empty;

		public List<GameEvent> Events { get; set; } = new List<GameEvent>();

		public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;
	}
}
