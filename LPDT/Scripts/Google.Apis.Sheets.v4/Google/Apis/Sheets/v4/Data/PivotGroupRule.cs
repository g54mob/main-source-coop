using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotGroupRule : IDirectResponseSchema
	{
		[JsonProperty("dateTimeRule")]
		public virtual DateTimeRule DateTimeRule { get; set; }

		[JsonProperty("histogramRule")]
		public virtual HistogramRule HistogramRule { get; set; }

		[JsonProperty("manualRule")]
		public virtual ManualRule ManualRule { get; set; }

		public virtual string ETag { get; set; }
	}
}
