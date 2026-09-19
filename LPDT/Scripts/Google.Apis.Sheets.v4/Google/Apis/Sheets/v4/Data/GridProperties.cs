using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class GridProperties : IDirectResponseSchema
	{
		[JsonProperty("columnCount")]
		public virtual int? ColumnCount { get; set; }

		[JsonProperty("columnGroupControlAfter")]
		public virtual bool? ColumnGroupControlAfter { get; set; }

		[JsonProperty("frozenColumnCount")]
		public virtual int? FrozenColumnCount { get; set; }

		[JsonProperty("frozenRowCount")]
		public virtual int? FrozenRowCount { get; set; }

		[JsonProperty("hideGridlines")]
		public virtual bool? HideGridlines { get; set; }

		[JsonProperty("rowCount")]
		public virtual int? RowCount { get; set; }

		[JsonProperty("rowGroupControlAfter")]
		public virtual bool? RowGroupControlAfter { get; set; }

		public virtual string ETag { get; set; }
	}
}
