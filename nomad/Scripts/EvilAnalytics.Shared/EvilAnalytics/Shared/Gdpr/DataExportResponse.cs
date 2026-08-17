using System;
using System.Collections.Generic;

namespace EvilAnalytics.Shared.Gdpr
{
	public class DataExportResponse
	{
		public Guid PlayerId { get; set; }

		public string DeviceId { get; set; } = string.Empty;

		public DateTimeOffset ExportedAt { get; set; }

		public PlayerExportData? Player { get; set; }

		public List<SessionExportData>? Sessions { get; set; }

		public List<EventExportData>? Events { get; set; }

		public List<HardwareExportData>? Hardware { get; set; }
	}
}
