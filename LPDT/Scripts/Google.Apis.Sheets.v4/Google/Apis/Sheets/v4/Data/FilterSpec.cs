using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class FilterSpec : IDirectResponseSchema
	{
		[JsonProperty("columnIndex")]
		public virtual int? ColumnIndex { get; set; }

		[JsonProperty("dataSourceColumnReference")]
		public virtual DataSourceColumnReference DataSourceColumnReference { get; set; }

		[JsonProperty("filterCriteria")]
		public virtual FilterCriteria FilterCriteria { get; set; }

		public virtual string ETag { get; set; }
	}
}
