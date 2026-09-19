using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class EmbeddedObjectBorder : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual Color Color { get; set; }

		[JsonProperty("colorStyle")]
		public virtual ColorStyle ColorStyle { get; set; }

		public virtual string ETag { get; set; }
	}
}
