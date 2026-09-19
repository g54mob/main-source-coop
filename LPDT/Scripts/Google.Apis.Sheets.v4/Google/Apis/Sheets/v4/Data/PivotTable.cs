using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotTable : IDirectResponseSchema
	{
		[JsonProperty("columns")]
		public virtual IList<PivotGroup> Columns { get; set; }

		[JsonProperty("criteria")]
		public virtual IDictionary<string, PivotFilterCriteria> Criteria { get; set; }

		[JsonProperty("dataExecutionStatus")]
		public virtual DataExecutionStatus DataExecutionStatus { get; set; }

		[JsonProperty("dataSourceId")]
		public virtual string DataSourceId { get; set; }

		[JsonProperty("filterSpecs")]
		public virtual IList<PivotFilterSpec> FilterSpecs { get; set; }

		[JsonProperty("rows")]
		public virtual IList<PivotGroup> Rows { get; set; }

		[JsonProperty("source")]
		public virtual GridRange Source { get; set; }

		[JsonProperty("valueLayout")]
		public virtual string ValueLayout { get; set; }

		[JsonProperty("values")]
		public virtual IList<PivotValue> Values { get; set; }

		public virtual string ETag { get; set; }
	}
}
