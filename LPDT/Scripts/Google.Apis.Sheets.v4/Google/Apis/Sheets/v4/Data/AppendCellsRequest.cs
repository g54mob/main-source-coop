using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AppendCellsRequest : IDirectResponseSchema
	{
		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("rows")]
		public virtual IList<RowData> Rows { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
