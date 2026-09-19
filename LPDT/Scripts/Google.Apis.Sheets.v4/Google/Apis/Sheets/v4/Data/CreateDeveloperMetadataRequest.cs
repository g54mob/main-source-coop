using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class CreateDeveloperMetadataRequest : IDirectResponseSchema
	{
		[JsonProperty("developerMetadata")]
		public virtual DeveloperMetadata DeveloperMetadata { get; set; }

		public virtual string ETag { get; set; }
	}
}
