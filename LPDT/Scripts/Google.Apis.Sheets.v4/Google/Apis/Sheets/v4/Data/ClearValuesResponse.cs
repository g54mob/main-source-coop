using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ClearValuesResponse : IDirectResponseSchema
	{
		[JsonProperty("clearedRange")]
		public virtual string ClearedRange { get; set; }

		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
