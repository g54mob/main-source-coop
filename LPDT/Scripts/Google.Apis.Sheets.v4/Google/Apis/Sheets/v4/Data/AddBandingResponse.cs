using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AddBandingResponse : IDirectResponseSchema
	{
		[JsonProperty("bandedRange")]
		public virtual BandedRange BandedRange { get; set; }

		public virtual string ETag { get; set; }
	}
}
