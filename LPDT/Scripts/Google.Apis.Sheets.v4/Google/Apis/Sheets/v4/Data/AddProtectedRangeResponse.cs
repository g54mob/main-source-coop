using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddProtectedRangeResponse : IDirectResponseSchema
	{
		[JsonProperty("protectedRange")]
		public virtual ProtectedRange ProtectedRange { get; set; }

		public virtual string ETag { get; set; }
	}
}
