using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ColorStyle : IDirectResponseSchema
	{
		[JsonProperty("rgbColor")]
		public virtual Color RgbColor { get; set; }

		[JsonProperty("themeColor")]
		public virtual string ThemeColor { get; set; }

		public virtual string ETag { get; set; }
	}
}
