using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotGroup : IDirectResponseSchema
	{
		[JsonProperty("dataSourceColumnReference")]
		public virtual DataSourceColumnReference DataSourceColumnReference { get; set; }

		[JsonProperty("groupLimit")]
		public virtual PivotGroupLimit GroupLimit { get; set; }

		[JsonProperty("groupRule")]
		public virtual PivotGroupRule GroupRule { get; set; }

		[JsonProperty("label")]
		public virtual string Label { get; set; }

		[JsonProperty("repeatHeadings")]
		public virtual bool? RepeatHeadings { get; set; }

		[JsonProperty("showTotals")]
		public virtual bool? ShowTotals { get; set; }

		[JsonProperty("sortOrder")]
		public virtual string SortOrder { get; set; }

		[JsonProperty("sourceColumnOffset")]
		public virtual int? SourceColumnOffset { get; set; }

		[JsonProperty("valueBucket")]
		public virtual PivotGroupSortValueBucket ValueBucket { get; set; }

		[JsonProperty("valueMetadata")]
		public virtual IList<PivotGroupValueMetadata> ValueMetadata { get; set; }

		public virtual string ETag { get; set; }
	}
}
