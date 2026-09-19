using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteEmbeddedObjectRequest : IDirectResponseSchema
	{
		[JsonProperty("objectId")]
		public virtual int? ObjectId { get; set; }

		public virtual string ETag { get; set; }
	}
}
