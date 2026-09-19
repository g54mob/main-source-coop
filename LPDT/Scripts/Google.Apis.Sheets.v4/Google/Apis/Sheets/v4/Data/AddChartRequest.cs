using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddChartRequest : IDirectResponseSchema
	{
		[JsonProperty("chart")]
		public virtual EmbeddedChart Chart { get; set; }

		public virtual string ETag { get; set; }
	}
}
