using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class WaterfallChartDomain : IDirectResponseSchema
	{
		[JsonProperty("data")]
		public virtual ChartData Data { get; set; }

		[JsonProperty("reversed")]
		public virtual bool? Reversed { get; set; }

		public virtual string ETag { get; set; }
	}
}
