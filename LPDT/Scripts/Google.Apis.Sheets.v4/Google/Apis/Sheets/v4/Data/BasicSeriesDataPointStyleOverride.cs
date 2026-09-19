using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BasicSeriesDataPointStyleOverride : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual Color Color { get; set; }

		[JsonProperty("colorStyle")]
		public virtual ColorStyle ColorStyle { get; set; }

		[JsonProperty("index")]
		public virtual int? Index { get; set; }

		[JsonProperty("pointStyle")]
		public virtual PointStyle PointStyle { get; set; }

		public virtual string ETag { get; set; }
	}
}
