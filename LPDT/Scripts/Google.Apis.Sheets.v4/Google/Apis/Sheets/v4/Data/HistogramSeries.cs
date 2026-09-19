using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class HistogramSeries : IDirectResponseSchema
	{
		[JsonProperty("barColor")]
		public virtual Color BarColor { get; set; }

		[JsonProperty("barColorStyle")]
		public virtual ColorStyle BarColorStyle { get; set; }

		[JsonProperty("data")]
		public virtual ChartData Data { get; set; }

		public virtual string ETag { get; set; }
	}
}
