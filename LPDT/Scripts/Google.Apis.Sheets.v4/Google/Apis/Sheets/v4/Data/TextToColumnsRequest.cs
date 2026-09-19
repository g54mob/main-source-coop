using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TextToColumnsRequest : IDirectResponseSchema
	{
		[JsonProperty("delimiter")]
		public virtual string Delimiter { get; set; }

		[JsonProperty("delimiterType")]
		public virtual string DelimiterType { get; set; }

		[JsonProperty("source")]
		public virtual GridRange Source { get; set; }

		public virtual string ETag { get; set; }
	}
}
