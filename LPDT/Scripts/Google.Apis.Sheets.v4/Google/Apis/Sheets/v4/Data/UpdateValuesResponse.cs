using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateValuesResponse : IDirectResponseSchema
	{
		[JsonProperty("spreadsheetId")]
		public virtual string SpreadsheetId { get; set; }

		[JsonProperty("updatedCells")]
		public virtual int? UpdatedCells { get; set; }

		[JsonProperty("updatedColumns")]
		public virtual int? UpdatedColumns { get; set; }

		[JsonProperty("updatedData")]
		public virtual ValueRange UpdatedData { get; set; }

		[JsonProperty("updatedRange")]
		public virtual string UpdatedRange { get; set; }

		[JsonProperty("updatedRows")]
		public virtual int? UpdatedRows { get; set; }

		public virtual string ETag { get; set; }
	}
}
