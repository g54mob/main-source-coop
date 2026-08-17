using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class DataExportRequest
	{
		public string DeviceId { get; set; } = string.Empty;

		[Obsolete("API key should be sent via X-API-Key header only. This field is ignored.")]
		public string ApiKey { get; set; } = string.Empty;

		public bool IncludeEvents { get; set; } = true;

		public bool IncludeSessions { get; set; } = true;

		public bool IncludeHardware { get; set; } = true;
	}
}
