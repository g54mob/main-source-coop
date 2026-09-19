using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class BandingProperties : IDirectResponseSchema
	{
		[JsonProperty("firstBandColor")]
		public virtual Color FirstBandColor { get; set; }

		[JsonProperty("firstBandColorStyle")]
		public virtual ColorStyle FirstBandColorStyle { get; set; }

		[JsonProperty("footerColor")]
		public virtual Color FooterColor { get; set; }

		[JsonProperty("footerColorStyle")]
		public virtual ColorStyle FooterColorStyle { get; set; }

		[JsonProperty("headerColor")]
		public virtual Color HeaderColor { get; set; }

		[JsonProperty("headerColorStyle")]
		public virtual ColorStyle HeaderColorStyle { get; set; }

		[JsonProperty("secondBandColor")]
		public virtual Color SecondBandColor { get; set; }

		[JsonProperty("secondBandColorStyle")]
		public virtual ColorStyle SecondBandColorStyle { get; set; }

		public virtual string ETag { get; set; }
	}
}
