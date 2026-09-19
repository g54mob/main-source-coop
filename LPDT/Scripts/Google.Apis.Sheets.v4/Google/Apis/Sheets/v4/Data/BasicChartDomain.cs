using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BasicChartDomain : IDirectResponseSchema
	{
		[JsonProperty("domain")]
		public virtual ChartData Domain { get; set; }

		[JsonProperty("reversed")]
		public virtual bool? Reversed { get; set; }

		public virtual string ETag { get; set; }
	}
}
