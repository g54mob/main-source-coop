using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataExecutionStatus : IDirectResponseSchema
	{
		[JsonProperty("errorCode")]
		public virtual string ErrorCode { get; set; }

		[JsonProperty("errorMessage")]
		public virtual string ErrorMessage { get; set; }

		[JsonProperty("lastRefreshTime")]
		public virtual object LastRefreshTime { get; set; }

		[JsonProperty("state")]
		public virtual string State { get; set; }

		public virtual string ETag { get; set; }
	}
}
