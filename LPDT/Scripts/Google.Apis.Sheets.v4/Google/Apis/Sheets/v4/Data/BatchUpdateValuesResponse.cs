using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BatchUpdateValuesResponse : IDirectResponseSchema
	{
		[JsonProperty("responses")]
		public virtual IList<UpdateValuesResponse> Responses { get; set; }

		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		[JsonProperty("totalUpdatedCells")]
		public virtual int? TotalUpdatedCells { get; set; }

		[JsonProperty("totalUpdatedColumns")]
		public virtual int? TotalUpdatedColumns { get; set; }

		[JsonProperty("totalUpdatedRows")]
		public virtual int? TotalUpdatedRows { get; set; }

		[JsonProperty("totalUpdatedSheets")]
		public virtual int? TotalUpdatedSheets { get; set; }

		public virtual string ETag { get; set; }
	}
}
