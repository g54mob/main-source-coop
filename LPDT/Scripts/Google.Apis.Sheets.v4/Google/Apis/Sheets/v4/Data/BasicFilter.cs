using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BasicFilter : IDirectResponseSchema
	{
		[JsonProperty("criteria")]
		public virtual IDictionary<string, FilterCriteria> Criteria { get; set; }

		[JsonProperty("filterSpecs")]
		public virtual IList<FilterSpec> FilterSpecs { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("sortSpecs")]
		public virtual IList<SortSpec> SortSpecs { get; set; }

		public virtual string ETag { get; set; }
	}
}
