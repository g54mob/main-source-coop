using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceRefreshSchedule : IDirectResponseSchema
	{
		[JsonProperty("dailySchedule")]
		public virtual DataSourceRefreshDailySchedule DailySchedule { get; set; }

		[JsonProperty("enabled")]
		public virtual bool? Enabled { get; set; }

		[JsonProperty("monthlySchedule")]
		public virtual DataSourceRefreshMonthlySchedule MonthlySchedule { get; set; }

		[JsonProperty("nextRun")]
		public virtual Interval NextRun { get; set; }

		[JsonProperty("refreshScope")]
		public virtual string RefreshScope { get; set; }

		[JsonProperty("weeklySchedule")]
		public virtual DataSourceRefreshWeeklySchedule WeeklySchedule { get; set; }

		public virtual string ETag { get; set; }
	}
}
