using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class DeleteBandingRequest : IDirectResponseSchema
	{
		[JsonProperty("bandedRangeId")]
		public virtual int? BandedRangeId { get; set; }

		public virtual string ETag { get; set; }
	}
}
