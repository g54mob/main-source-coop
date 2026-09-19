using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CandlestickData : IDirectResponseSchema
	{
		[JsonProperty("closeSeries")]
		public virtual CandlestickSeries CloseSeries { get; set; }

		[JsonProperty("highSeries")]
		public virtual CandlestickSeries HighSeries { get; set; }

		[JsonProperty("lowSeries")]
		public virtual CandlestickSeries LowSeries { get; set; }

		[JsonProperty("openSeries")]
		public virtual CandlestickSeries OpenSeries { get; set; }

		public virtual string ETag { get; set; }
	}
}
