using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateDeveloperMetadataResponse : IDirectResponseSchema
	{
		[JsonProperty("developerMetadata")]
		public virtual IList<DeveloperMetadata> DeveloperMetadata { get; set; }

		public virtual string ETag { get; set; }
	}
}
