using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class MoveDimensionRequest : IDirectResponseSchema
	{
		[JsonProperty("destinationIndex")]
		public virtual int? DestinationIndex { get; set; }

		[JsonProperty("source")]
		public virtual DimensionRange Source { get; set; }

		public virtual string ETag { get; set; }
	}
}
