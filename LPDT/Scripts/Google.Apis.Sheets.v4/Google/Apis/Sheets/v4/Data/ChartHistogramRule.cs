using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartHistogramRule : IDirectResponseSchema
	{
		[JsonProperty("intervalSize")]
		public virtual double? IntervalSize { get; set; }

		[JsonProperty("maxValue")]
		public virtual double? MaxValue { get; set; }

		[JsonProperty("minValue")]
		public virtual double? MinValue { get; set; }

		public virtual string ETag { get; set; }
	}
}
