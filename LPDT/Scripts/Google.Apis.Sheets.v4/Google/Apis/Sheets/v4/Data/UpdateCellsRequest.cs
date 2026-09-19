using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateCellsRequest : IDirectResponseSchema
	{
		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("rows")]
		public virtual IList<RowData> Rows { get; set; }

		[JsonProperty("start")]
		public virtual GridCoordinate Start { get; set; }

		public virtual string ETag { get; set; }
	}
}
