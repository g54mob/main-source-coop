using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotFilterSpec : IDirectResponseSchema
	{
		[JsonProperty("columnOffsetIndex")]
		public virtual int? ColumnOffsetIndex { get; set; }

		[JsonProperty("dataSourceColumnReference")]
		public virtual DataSourceColumnReference DataSourceColumnReference { get; set; }

		[JsonProperty("filterCriteria")]
		public virtual PivotFilterCriteria FilterCriteria { get; set; }

		public virtual string ETag { get; set; }
	}
}
