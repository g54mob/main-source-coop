using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ThemeColorPair : IDirectResponseSchema
	{
		[JsonProperty("color")]
		public virtual ColorStyle Color { get; set; }

		[JsonProperty("colorType")]
		public virtual string ColorType { get; set; }

		public virtual string ETag { get; set; }
	}
}
