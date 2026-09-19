using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchClearValuesByDataFilterResponse : IDirectResponseSchema
	{
		[JsonProperty("clearedRanges")]
		public virtual IList<string> ClearedRanges { get; set; }

		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
