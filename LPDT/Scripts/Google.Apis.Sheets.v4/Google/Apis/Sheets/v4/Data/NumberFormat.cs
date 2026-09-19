using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class NumberFormat : IDirectResponseSchema
	{
		[JsonProperty("pattern")]
		public virtual string Pattern { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
