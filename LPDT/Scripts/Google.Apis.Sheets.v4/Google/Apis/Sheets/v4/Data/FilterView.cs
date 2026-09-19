using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class FilterView : IDirectResponseSchema
	{
		[JsonProperty("criteria")]
		public virtual IDictionary<string, FilterCriteria> Criteria { get; set; }

		[JsonProperty("filterSpecs")]
		public virtual IList<FilterSpec> FilterSpecs { get; set; }

		[JsonProperty("filterViewId")]
		public virtual int? FilterViewId { get; set; }

		[JsonProperty("namedRangeId")]
		public virtual string NamedRangeId { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("sortSpecs")]
		public virtual IList<SortSpec> SortSpecs { get; set; }

		[JsonProperty("title")]
		public virtual string Title { get; set; }

		public virtual string ETag { get; set; }
	}
}
