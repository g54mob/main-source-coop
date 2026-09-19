using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CandlestickSeries : IDirectResponseSchema
	{
		[JsonProperty("data")]
		public virtual ChartData Data { get; set; }

		public virtual string ETag { get; set; }
	}
}
