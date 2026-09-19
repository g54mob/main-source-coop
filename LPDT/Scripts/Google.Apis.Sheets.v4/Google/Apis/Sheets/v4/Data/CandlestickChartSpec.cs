using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CandlestickChartSpec : IDirectResponseSchema
	{
		[JsonProperty("data")]
		public virtual IList<CandlestickData> Data { get; set; }

		[JsonProperty("domain")]
		public virtual CandlestickDomain Domain { get; set; }

		public virtual string ETag { get; set; }
	}
}
