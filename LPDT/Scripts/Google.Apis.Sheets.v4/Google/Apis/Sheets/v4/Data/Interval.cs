using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Interval : IDirectResponseSchema
	{
		[JsonProperty("endTime")]
		public virtual object EndTime { get; set; }

		[JsonProperty("startTime")]
		public virtual object StartTime { get; set; }

		public virtual string ETag { get; set; }
	}
}
