using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteDuplicatesRequest : IDirectResponseSchema
	{
		[JsonProperty("comparisonColumns")]
		public virtual IList<DimensionRange> ComparisonColumns { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
