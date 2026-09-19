using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SourceAndDestination : IDirectResponseSchema
	{
		[JsonProperty("dimension")]
		public virtual string Dimension { get; set; }

		[JsonProperty("fillLength")]
		public virtual int? FillLength { get; set; }

		[JsonProperty("source")]
		public virtual GridRange Source { get; set; }

		public virtual string ETag { get; set; }
	}
}
