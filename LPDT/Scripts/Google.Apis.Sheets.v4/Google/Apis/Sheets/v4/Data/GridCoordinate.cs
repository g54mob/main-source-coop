using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class GridCoordinate : IDirectResponseSchema
	{
		[JsonProperty("columnIndex")]
		public virtual int? ColumnIndex { get; set; }

		[JsonProperty("rowIndex")]
		public virtual int? RowIndex { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
