using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class ConsentUpdateRequest
	{
		public string DeviceId { get; set; } = string.Empty;

		[Obsolete("API key should be sent via X-API-Key header only. This field is ignored.")]
		public string ApiKey { get; set; } = string.Empty;

		public bool? Analytics { get; set; }

		public bool? Hardware { get; set; }

		public bool? Performance { get; set; }

		public bool? CrashReporting { get; set; }

		public string? ConsentVersion { get; set; }
	}
}
