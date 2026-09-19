using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateEmbeddedObjectBorderRequest : IDirectResponseSchema
	{
		[JsonProperty("border")]
		public virtual EmbeddedObjectBorder Border { get; set; }

		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		[JsonProperty("objectId")]
		public virtual int? ObjectId { get; set; }

		public virtual string ETag { get; set; }
	}
}
