using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteNamedRangeRequest : IDirectResponseSchema
	{
		[JsonProperty("namedRangeId")]
		public virtual string NamedRangeId { get; set; }

		public virtual string ETag { get; set; }
	}
}
