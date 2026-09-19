using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PivotFilterCriteria : IDirectResponseSchema
	{
		[JsonProperty("condition")]
		public virtual BooleanCondition Condition { get; set; }

		[JsonProperty("visibleByDefault")]
		public virtual bool? VisibleByDefault { get; set; }

		[JsonProperty("visibleValues")]
		public virtual IList<string> VisibleValues { get; set; }

		public virtual string ETag { get; set; }
	}
}
