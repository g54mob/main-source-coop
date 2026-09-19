using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteDeveloperMetadataResponse : IDirectResponseSchema
	{
		[JsonProperty("deletedDeveloperMetadata")]
		public virtual IList<DeveloperMetadata> DeletedDeveloperMetadata { get; set; }

		public virtual string ETag { get; set; }
	}
}
