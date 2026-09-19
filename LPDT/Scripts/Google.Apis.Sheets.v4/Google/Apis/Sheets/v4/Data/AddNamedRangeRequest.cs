using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddNamedRangeRequest : IDirectResponseSchema
	{
		[JsonProperty("namedRange")]
		public virtual NamedRange NamedRange { get; set; }

		public virtual string ETag { get; set; }
	}
}
