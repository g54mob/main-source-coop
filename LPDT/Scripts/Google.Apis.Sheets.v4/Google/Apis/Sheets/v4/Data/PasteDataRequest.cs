using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class PasteDataRequest : IDirectResponseSchema
	{
		[JsonProperty("coordinate")]
		public virtual GridCoordinate Coordinate { get; set; }

		[JsonProperty("data")]
		public virtual string Data { get; set; }

		[JsonProperty("delimiter")]
		public virtual string Delimiter { get; set; }

		[JsonProperty("html")]
		public virtual bool? Html { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
