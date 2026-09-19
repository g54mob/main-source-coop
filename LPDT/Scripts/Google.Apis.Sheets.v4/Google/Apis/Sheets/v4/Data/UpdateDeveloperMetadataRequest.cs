using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateDeveloperMetadataRequest : IDirectResponseSchema
	{
		[JsonProperty("dataFilters")]
		public virtual IList<DataFilter> DataFilters { get; set; }

		[JsonProperty("developerMetadata")]
		public virtual DeveloperMetadata DeveloperMetadata { get; set; }

		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		public virtual string ETag { get; set; }
	}
}
