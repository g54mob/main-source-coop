using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteProtectedRangeRequest : IDirectResponseSchema
	{
		[JsonProperty("protectedRangeId")]
		public virtual int? ProtectedRangeId { get; set; }

		public virtual string ETag { get; set; }
	}
}
