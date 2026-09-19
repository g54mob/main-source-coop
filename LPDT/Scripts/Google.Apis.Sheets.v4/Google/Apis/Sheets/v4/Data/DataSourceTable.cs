using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataSourceTable : IDirectResponseSchema
	{
		[JsonProperty("columnSelectionType")]
		public virtual string ColumnSelectionType { get; set; }

		[JsonProperty("columns")]
		public virtual IList<DataSourceColumnReference> Columns { get; set; }

		[JsonProperty("dataExecutionStatus")]
		public virtual DataExecutionStatus DataExecutionStatus { get; set; }

		[JsonProperty("dataSourceId")]
		public virtual string DataSourceId { get; set; }

		[JsonProperty("filterSpecs")]
		public virtual IList<FilterSpec> FilterSpecs { get; set; }

		[JsonProperty("rowLimit")]
		public virtual int? RowLimit { get; set; }

		[JsonProperty("sortSpecs")]
		public virtual IList<SortSpec> SortSpecs { get; set; }

		public virtual string ETag { get; set; }
	}
}
