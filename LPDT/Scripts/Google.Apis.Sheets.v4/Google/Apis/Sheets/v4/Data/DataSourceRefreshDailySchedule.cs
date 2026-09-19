using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceRefreshDailySchedule : IDirectResponseSchema
	{
		[JsonProperty("startTime")]
		public virtual TimeOfDay StartTime { get; set; }

		public virtual string ETag { get; set; }
	}
}
