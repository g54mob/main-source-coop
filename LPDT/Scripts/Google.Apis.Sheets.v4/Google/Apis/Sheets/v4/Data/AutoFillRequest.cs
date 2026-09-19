using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class AutoFillRequest : IDirectResponseSchema
	{
		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("sourceAndDestination")]
		public virtual SourceAndDestination SourceAndDestination { get; set; }

		[JsonProperty("useAlternateSeries")]
		public virtual bool? UseAlternateSeries { get; set; }

		public virtual string ETag { get; set; }
	}
}
