using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class GridData : IDirectResponseSchema
	{
		[JsonProperty("columnMetadata")]
		public virtual IList<DimensionProperties> ColumnMetadata { get; set; }

		[JsonProperty("rowData")]
		public virtual IList<RowData> RowData { get; set; }

		[JsonProperty("rowMetadata")]
		public virtual IList<DimensionProperties> RowMetadata { get; set; }

		[JsonProperty("startColumn")]
		public virtual int? StartColumn { get; set; }

		[JsonProperty("startRow")]
		public virtual int? StartRow { get; set; }

		public virtual string ETag { get; set; }
	}
}
