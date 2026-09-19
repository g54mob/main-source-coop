using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class WaterfallChartColumnStyle : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual Color Color { get; set; }

		[JsonProperty("colorStyle")]
		public virtual ColorStyle ColorStyle { get; set; }

		[JsonProperty("label")]
		public virtual string Label { get; set; }

		public virtual string ETag { get; set; }
	}
}
