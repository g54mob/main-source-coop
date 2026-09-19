using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchGetValuesByDataFilterResponse : IDirectResponseSchema
	{
		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		[JsonProperty("valueRanges")]
		public virtual IList<MatchedValueRange> ValueRanges { get; set; }

		public virtual string ETag { get; set; }
	}
}
