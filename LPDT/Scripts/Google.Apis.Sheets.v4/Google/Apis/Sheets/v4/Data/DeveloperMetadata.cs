using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeveloperMetadata : IDirectResponseSchema
	{
		[JsonProperty("location")]
		public virtual DeveloperMetadataLocation Location { get; set; }

		[JsonProperty("metadataId")]
		public virtual int? MetadataId { get; set; }

		[JsonProperty("metadataKey")]
		public virtual string MetadataKey { get; set; }

		[JsonProperty("metadataValue")]
		public virtual string MetadataValue { get; set; }

		[JsonProperty("visibility")]
		public virtual string Visibility { get; set; }

		public virtual string ETag { get; set; }
	}
}
