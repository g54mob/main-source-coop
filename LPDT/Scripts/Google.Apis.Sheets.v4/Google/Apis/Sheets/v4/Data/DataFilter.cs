using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DataFilter : IDirectResponseSchema
	{
		[JsonProperty("a1Range")]
		public virtual string A1Range { get; set; }

		[JsonProperty("developerMetadataLookup")]
		public virtual DeveloperMetadataLookup DeveloperMetadataLookup { get; set; }

		[JsonProperty("gridRange")]
		public virtual GridRange GridRange { get; set; }

		public virtual string ETag { get; set; }
	}
}
