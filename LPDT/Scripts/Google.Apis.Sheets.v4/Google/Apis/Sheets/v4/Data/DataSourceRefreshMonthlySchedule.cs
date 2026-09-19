using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceRefreshMonthlySchedule : IDirectResponseSchema
	{
		[JsonProperty("daysOfMonth")]
		public virtual IList<int?> DaysOfMonth { get; set; }

		[JsonProperty("startTime")]
		public virtual TimeOfDay StartTime { get; set; }

		public virtual string ETag { get; set; }
	}
}
