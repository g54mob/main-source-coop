using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class MatchedDeveloperMetadata : IDirectResponseSchema
	{
		[JsonProperty("dataFilters")]
		public virtual IList<DataFilter> DataFilters { get; set; }

		[JsonProperty("developerMetadata")]
		public virtual DeveloperMetadata DeveloperMetadata { get; set; }

		public virtual string ETag { get; set; }
	}
}
