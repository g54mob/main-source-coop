using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SearchDeveloperMetadataResponse : IDirectResponseSchema
	{
		[JsonProperty("matchedDeveloperMetadata")]
		public virtual IList<MatchedDeveloperMetadata> MatchedDeveloperMetadata { get; set; }

		public virtual string ETag { get; set; }
	}
}
