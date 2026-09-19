using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AppendValuesResponse : IDirectResponseSchema
	{
		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		[JsonProperty("tableRange")]
		public virtual string TableRange { get; set; }

		[JsonProperty("updates")]
		public virtual UpdateValuesResponse Updates { get; set; }

		public virtual string ETag { get; set; }
	}
}
