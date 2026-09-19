using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchUpdateSpreadsheetResponse : IDirectResponseSchema
	{
		[JsonProperty("replies")]
		public virtual IList<Response> Replies { get; set; }

		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		[JsonProperty("updatedSpreadsheet")]
		public virtual Spreadsheet UpdatedSpreadsheet { get; set; }

		public virtual string ETag { get; set; }
	}
}
