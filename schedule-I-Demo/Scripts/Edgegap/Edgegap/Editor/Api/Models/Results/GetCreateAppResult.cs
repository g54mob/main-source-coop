using Newtonsoft.Json;

namespace Edgegap.Editor.Api.Models.Results
{
	public class GetCreateAppResult
	{
		[JsonProperty("name")]
		public string AppName { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("is_telemetry_agent_active")]
		public bool IsTelemetryAgentActive { get; set; }

		[JsonProperty("image")]
		public string Image { get; set; }

		[JsonProperty("create_time")]
		public string CreateTimeStr { get; set; }

		[JsonProperty("last_updated")]
		public string LastUpdatedStr { get; set; }
	}
}
