using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateEmbeddedObjectPositionRequest : IDirectResponseSchema
	{
		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("newPosition")]
		public virtual EmbeddedObjectPosition NewPosition { get; set; }

		[JsonProperty("objectId")]
		public virtual int? ObjectId { get; set; }

		public virtual string ETag { get; set; }
	}
}
