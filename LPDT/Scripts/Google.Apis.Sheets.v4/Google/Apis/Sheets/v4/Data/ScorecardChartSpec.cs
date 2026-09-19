using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ScorecardChartSpec : IDirectResponseSchema
	{
		[JsonProperty("aggregateType")]
		public virtual string AggregateType { get; set; }

		[JsonProperty("baselineValueData")]
		public virtual ChartData BaselineValueData { get; set; }

		[JsonProperty("baselineValueFormat")]
		public virtual BaselineValueFormat BaselineValueFormat { get; set; }

		[JsonProperty("customFormatOptions")]
		public virtual ChartCustomNumberFormatOptions CustomFormatOptions { get; set; }

		[JsonProperty("keyValueData")]
		public virtual ChartData KeyValueData { get; set; }

		[JsonProperty("keyValueFormat")]
		public virtual KeyValueFormat KeyValueFormat { get; set; }

		[JsonProperty("numberFormatSource")]
		public virtual string NumberFormatSource { get; set; }

		[JsonProperty("scaleFactor")]
		public virtual double? ScaleFactor { get; set; }

		public virtual string ETag { get; set; }
	}
}
