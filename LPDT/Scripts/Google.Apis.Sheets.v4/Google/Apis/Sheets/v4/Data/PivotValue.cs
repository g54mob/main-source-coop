using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotValue : IDirectResponseSchema
	{
		[JsonProperty("calculatedDisplayType")]
		public virtual string CalculatedDisplayType { get; set; }

		[JsonProperty("dataSourceColumnReference")]
		public virtual DataSourceColumnReference DataSourceColumnReference { get; set; }

		[JsonProperty("formula")]
		public virtual string Formula { get; set; }

		[JsonProperty("name")]
		public virtual string Name { get; set; }

		[JsonProperty("sourceColumnOffset")]
		public virtual int? SourceColumnOffset { get; set; }

		[JsonProperty("summarizeFunction")]
		public virtual string SummarizeFunction { get; set; }

		public virtual string ETag { get; set; }
	}
}
