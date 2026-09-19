using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class InterpolationPoint : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual Color Color { get; set; }

		[JsonProperty("colorStyle")]
		public virtual ColorStyle ColorStyle { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		[JsonProperty("value")]
		public virtual string Value { get; set; }

		public virtual string ETag { get; set; }
	}
}
