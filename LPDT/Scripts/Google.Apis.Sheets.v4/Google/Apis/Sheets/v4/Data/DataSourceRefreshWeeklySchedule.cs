using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceRefreshWeeklySchedule : IDirectResponseSchema
	{
		[JsonProperty("daysOfWeek")]
		public virtual IList<string> DaysOfWeek { get; set; }

		[JsonProperty("startTime")]
		public virtual TimeOfDay StartTime { get; set; }

		public virtual string ETag { get; set; }
	}
}
