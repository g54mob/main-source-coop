using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ChartData : IDirectResponseSchema
	{
		[JsonProperty("aggregateType")]
		public virtual string AggregateType { get; set; }

		[JsonProperty("columnReference")]
		public virtual DataSourceColumnReference ColumnReference { get; set; }

		[JsonProperty("groupRule")]
		public virtual ChartGroupRule GroupRule { get; set; }

		[JsonProperty("sourceRange")]
		public virtual ChartSourceRange SourceRange { get; set; }

		public virtual string ETag { get; set; }
	}
}
