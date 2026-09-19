using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Border : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual Color Color { get; set; }

		[JsonProperty("colorStyle")]
		public virtual ColorStyle ColorStyle { get; set; }

		[JsonProperty("style")]
		public virtual string Style { get; set; }

		[JsonProperty("width")]
		public virtual int? Width { get; set; }

		public virtual string ETag { get; set; }
	}
}
