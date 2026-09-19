using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class HistogramRule : IDirectResponseSchema
	{
		[JsonProperty("end")]
		public virtual double? End { get; set; }

		[JsonProperty("interval")]
		public virtual double? Interval { get; set; }

		[JsonProperty("start")]
		public virtual double? Start { get; set; }

		public virtual string ETag { get; set; }
	}
}
