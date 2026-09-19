using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class GridRange : IDirectResponseSchema
	{
		[JsonProperty("endColumnIndex")]
		public virtual int? EndColumnIndex { get; set; }

		[JsonProperty("endRowIndex")]
		public virtual int? EndRowIndex { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		[JsonProperty("startColumnIndex")]
		public virtual int? StartColumnIndex { get; set; }

		[JsonProperty("startRowIndex")]
		public virtual int? StartRowIndex { get; set; }

		public virtual string ETag { get; set; }
	}
}
