using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartGroupRule : IDirectResponseSchema
	{
		[JsonProperty("dateTimeRule")]
		public virtual ChartDateTimeRule DateTimeRule { get; set; }

		[JsonProperty("histogramRule")]
		public virtual ChartHistogramRule HistogramRule { get; set; }

		public virtual string ETag { get; set; }
	}
}
