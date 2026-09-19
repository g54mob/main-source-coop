using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class UpdateBandingRequest : IDirectResponseSchema
	{
		[JsonProperty("bandedRange")]
		public virtual BandedRange BandedRange { get; set; }

		[JsonProperty("fields")]
		public virtual object Fields { get; set; }

		public virtual string ETag { get; set; }
	}
}
